import React, { useEffect, useState, useMemo, useCallback } from "react";
import CustomModal from "../../static/CustomModal/CustomModal";
import CustomButton from "../../static/Commons/CustomButton";
import CustomSelect from "../../static/Dropdown/CustomSelect";
import useApiClients from "../../../services/useApiClients";
import useCurrentUserId from "../../../hooks/useCurrentUserId";
import useFormHandler from "../../../hooks/useFormHandler";
import useToastMessage from "../../../hooks/useToastMessage";
import RewardPopup from "../../Reward/RewardPopup";


const AddEditTestCenterModal = ({
    isOpen,
    modalType,
    onClose,
    onAdd,
    testCenterAndTestListData,
    doctorRecommendedTestListData,
    doctorRecommendedTestCenterAndTestListData,
    SmartRxMasterId,
    PrescriptionId,
    LoginUserId,
    smartRxInsiderDataRefetch,
}) => {
    const { api } = useApiClients();
	const currentUserId = useCurrentUserId();
    const { resetForm } = useFormHandler();
    const showToast = useToastMessage();

    // State
    const [formData, setFormData] = useState({ DiagnosticCenterName: "", Branch: "", InvestigationId: "" });
    const [fieldErrors, setFieldErrors] = useState({});
    const [selectedTests, setSelectedTests] = useState({});
    const [addedCenters, setAddedCenters] = useState([]);
    const [centerRemoveError, setCenterRemoveError] = useState("");
    const [isLoading, setIsLoading] = useState(false);
    const [doctorRecommendedCenters, setDoctorRecommendedCenters] = useState([]);
    const [showRewardPopup, setShowRewardPopup] = useState(false);
    const [uploadResponse, setUploadResponse] = useState(null);

    // Memoized data
    const uniqueTestCenters = useMemo(() =>
        Array.from(new Map(testCenterAndTestListData?.testCenters?.map(item => [item.testCenterId, item])).values()),
        [testCenterAndTestListData?.testCenters]
    );

    const testCenterOptions = useMemo(() =>
        uniqueTestCenters?.map(option => ({
            value: option.testCenterId,
            label: option.testCenterBranch ? `${option.testCenterName} - ${option.testCenterBranch}` : option.testCenterName,
        })),
        [uniqueTestCenters]
    );

    // Handlers
    const handleSelectChange = useCallback((e) => {
        const selectedId = parseInt(e.target.value);
        const selectedCenter = uniqueTestCenters.find(center => center.testCenterId === selectedId);

        setFormData(prev => ({
            ...prev,
            DiagnosticCenterName: e.target.value,
            Branch: selectedCenter?.testCenterBranch || "",
        }));
        setFieldErrors(prev => ({ ...prev, DiagnosticCenterName: null }));
    }, [uniqueTestCenters]);

    const toggleTestSelection = useCallback((centerId, testName) => {
        const key = `${centerId}_${testName}`;
        setSelectedTests(prev => ({ ...prev, [key]: !prev[key] }));
    }, []);

    const getTestSelectionCount = useCallback((testName) =>
        Object.keys(selectedTests).reduce((count, key) =>
            key.endsWith(`_${testName}`) && selectedTests[key] ? count + 1 : count, 0
        ), [selectedTests]
    );

    const handleRemoveCenter = useCallback((centerId) => {
        setAddedCenters(prev => prev.filter(entry => entry.center.testCenterId !== centerId));
        showToast("success", "Test center removed from the list", "");
    }, []);

    const handleRemoveDoctorRecommendedCenter = useCallback((centerIdToRemove) => {
        const centerIdToRemoveStr = centerIdToRemove.toString();
        
        const isAnySelected = Object.keys(selectedTests).some(key => {
            const [id] = key.split("_");
            return id === centerIdToRemoveStr && selectedTests[key];
        });

        if (isAnySelected) {
            setCenterRemoveError("Please unselect all tests from this center before removing.");
            setTimeout(() => setCenterRemoveError(""), 2000);
            return;
        }

        setDoctorRecommendedCenters(prev => {
            return prev.map(({ center, tests }) => {
                const updatedTests = tests.map(test => {

                    if (!test.testCenterIds || test.testCenterIds.trim() === "") {
                        return test;
                    }

                    const originalCenterIds = test.testCenterIds.split(",")
                        .map(id => id.trim())
                        .filter(id => id !== "");
                    
                    const hasCenterToRemove = originalCenterIds.includes(centerIdToRemoveStr);
                    
                    if (!hasCenterToRemove) {
                        return test;
                    }

                    const updatedCenterIds = originalCenterIds.filter(id => id !== centerIdToRemoveStr);
                
                    const originalTestCenters = [...(test?.testCenters || [])];
                    const updatedTestCenters = [];
                    
                    originalCenterIds.forEach((centerId, idx) => {
                        if (centerId !== centerIdToRemoveStr && idx < originalTestCenters.length) {
                            updatedTestCenters.push(originalTestCenters[idx]);
                        }
                    });
                    
                    if (originalTestCenters.length > originalCenterIds.length) {
                        updatedTestCenters.push(...originalTestCenters.slice(originalCenterIds.length));
                    }

                    if (updatedCenterIds.length === 0) {
                        return null;
                    }
                    
                    const finalTestCenters = updatedTestCenters.slice(0, updatedCenterIds.length);

                    return { 
                        ...test, 
                        testCenterIds: updatedCenterIds.join(","), 
                        testCenters: finalTestCenters 
                    };
                }).filter(test => test !== null);

                return updatedTests.length > 0 ? { center, tests: updatedTests } : null;
            }).filter(Boolean);
        });
        
        setSelectedTests(prev => {
            const updated = { ...prev };
            Object.keys(updated).forEach(key => {
                const [id] = key.split("_");
                if (id === centerIdToRemoveStr) {
                    delete updated[key];
                }
            });
            return updated;
        });
        
        showToast("success", "Test center removed from the list", "");
    }, [selectedTests, showToast]);

    const handleCenterBulkSelection = useCallback((testKeys, allSelected) => {
        const updated = { ...selectedTests };
        testKeys.forEach(key => { updated[key] = !allSelected; });
        setSelectedTests(updated);
    }, [selectedTests]);

    const doctorRecommendedCentersData = useMemo(() => {
        const allTests = doctorRecommendedCenters.flatMap(d => d.tests || []);
        const centerIdToName = {};
        uniqueTestCenters.forEach(center => {
            centerIdToName[center.testCenterId?.toString()] = center.testCenterName;
        });

        const centerIdMap = new Map();
        for (const test of allTests) {
            const idsArr = (test.testCenterIds || '').split(',').map(id => id.trim());
            idsArr.forEach((id) => {
                if (!id) return;
                if (!centerIdMap.has(id)) centerIdMap.set(id, []);
                if (!centerIdMap.get(id).some(t => t.testName === test.testName)) {
                    centerIdMap.get(id).push(test);
                }
            });
        }

        return Array.from(centerIdMap.entries()).map(([centerId, filteredTests]) => {
            const centerName = centerIdToName[centerId] || 'Unnamed Center';
            const testKeys = filteredTests.map(test => `${centerId}_${test.testName}`);
            const allSelected = testKeys.length > 0 && testKeys.every(key => selectedTests[key]);
            const anySelected = testKeys.some(key => selectedTests[key]);

            return filteredTests.length === 0 ? null : { centerId, centerName, filteredTests, testKeys, allSelected, anySelected };
        }).filter(Boolean);
    }, [doctorRecommendedCenters, uniqueTestCenters, selectedTests]);

    const totalCentersCount = doctorRecommendedCentersData.length + addedCenters.length;
    const isMaxCentersReached = totalCentersCount >= 2;

    const handleAddCenter = useCallback(() => {
        if (!formData.DiagnosticCenterName) return;

        // Check if max centers reached first
        if (isMaxCentersReached) {
            showToast("error", "You can add only 2 test center for free", "");
            return;
        }

        const centerId = parseInt(formData.DiagnosticCenterName);
        const selectedCenter = uniqueTestCenters.find(item => item.testCenterId === centerId);
        if (!selectedCenter) return;

        const alreadyExists = addedCenters.some(entry => entry.center.testCenterId === centerId);
        if (alreadyExists) return;

        const relatedTests = doctorRecommendedTestCenterAndTestListData?.flatMap(({ tests }) => tests || []) || [];
        setAddedCenters(prev => [...prev, { center: selectedCenter, tests: relatedTests }]);
        resetForm({ DiagnosticCenterName: "", Branch: "", InvestigationId: "" }, setFormData, setFieldErrors);
    }, [formData.DiagnosticCenterName, uniqueTestCenters, addedCenters, doctorRecommendedTestCenterAndTestListData, resetForm]);

    // Reset on modal close/open
    useEffect(() => {
        if (!isOpen) {
            resetForm({ DiagnosticCenterName: "", Branch: "", InvestigationId: "" }, setFormData, setFieldErrors);
            setSelectedTests({});
            setDoctorRecommendedCenters([]);
            setAddedCenters([]);
        } else {
            setDoctorRecommendedCenters(doctorRecommendedTestCenterAndTestListData || []);
            setAddedCenters([]);

            const initialSelections = {};
            (doctorRecommendedTestCenterAndTestListData || []).forEach(({ tests }) => {
                (tests || []).forEach(test => {
                    const ids = test?.testCenterIds?.split(",") || [];
                    ids.forEach(id => { initialSelections[`${id}_${test.testName}`] = true; });
                });
            });
            setSelectedTests(initialSelections);
        }
    }, [isOpen, doctorRecommendedTestCenterAndTestListData]);

    // Save selected centers
    const handleSaveSelectedCenters = async (e) => {
        e.preventDefault();
		const effectiveLoginUserId = LoginUserId ?? currentUserId ?? null;
        if (!SmartRxMasterId || !PrescriptionId || !effectiveLoginUserId) {
            console.error("Missing required IDs");
            return;
        }

        setIsLoading(true);
        try {
            const testToCentersMap = new Map();

            const processTest = (centerId, test) => {
                const testName = test?.testName?.trim();
                if (!testName || !centerId) return;

                const key = `${centerId}_${testName}`;
                if (!selectedTests?.[key]) return;

                const mapKey = testName;
                const matchingTest = doctorRecommendedTestCenterAndTestListData
                    .flatMap(c => c.tests || [])
                    .find(t => t.testName?.trim().toLowerCase() === testName.toLowerCase());

                const testId = test.testId || matchingTest?.testId || matchingTest?.id || 0;
                const investigationId = matchingTest?.id || 0;

                if (!testToCentersMap.has(mapKey)) {
                    testToCentersMap.set(mapKey, {
                        TestId: testId,
                        CenterIds: new Set([centerId]),
                        investigationId,
                    });
                } else {
                    testToCentersMap.get(mapKey).CenterIds.add(centerId);
                }
            };

            // Process centers
            doctorRecommendedCenters.forEach(({ tests }) => {
                tests?.forEach(test => {
                    test.testCenterIds?.split(",").forEach(cid => processTest(cid.trim(), test));
                });
            });

            addedCenters.forEach(({ center, tests }) => {
                const centerId = center?.testCenterId?.toString();
                tests?.forEach(test => processTest(centerId, test));
            });

            // Create payload
            const patientTestCenterWiseList = Array.from(testToCentersMap.values()).map(
                ({ TestId, CenterIds, investigationId }) => ({
                    Id: investigationId || 0,
                    SmartRxMasterId,
                    PrescriptionId,
                    TestId,
                    TestCenterIds: Array.from(CenterIds).slice(0, 2).join(","),
                })
            );

            const response = await api.investigationCenterListUpdate({
                SmartRxMasterId,
                PrescriptionId,
                PatientTestCenterWiseList: patientTestCenterWiseList,
                LoginUserId: effectiveLoginUserId,
            }, "");

            setUploadResponse(response);

            if (
                response?.message === "Successful" ||
                typeof response === "object"
            ) {
                if (
                    patientTestCenterWiseList.length > 0 && response?.response?.isRewardUpdated == true
                ) {
                    setShowRewardPopup(true);
                    setTimeout(() => {
                        setShowRewardPopup(false);
                        onClose();
                        smartRxInsiderDataRefetch();
                    }, 3000);
                } else {
                    onClose();
                    smartRxInsiderDataRefetch();
                }
            }

            //onClose();
            //smartRxInsiderDataRefetch();
        } catch (error) {
            console.error("Error saving centers:", error);
        } finally {
            setIsLoading(false);
        }
    };

    // Reusable components
    const TestItem = useCallback(({ centerId, test }) => {
        const key = `${centerId}_${test.testName}`;
        const isChecked = !!selectedTests[key];
        const selectedCount = getTestSelectionCount(test.testName);

        return (
            <div style={{ display: "flex", alignItems: "center", gap: "10px", minHeight: "28px", fontSize: "clamp(13px, 2vw, 16px)", padding: "4px 0" }}>
                <input
                    type="checkbox"
                    checked={isChecked}
                    onChange={() => toggleTestSelection(centerId, test.testName)}
                    disabled={!isChecked && selectedCount >= 2}
                    style={{ transform: "scale(1.2)", width: "clamp(5%, 30vw, 5%)", marginRight: "0" }}
                />
                <span style={{ width: "clamp(62%, 30vw, 62%)", textAlign: "left" }}>{test.testName}</span>
                <span style={{ textAlign: "left", borderLeft: "2px solid #000", paddingLeft: "15px", width: "clamp(33%, 30vw, 33%)" }}>
                    BDT {test.testUnitPrice ?? "N/A"}
                </span>
            </div>
        );
    }, [selectedTests, getTestSelectionCount, toggleTestSelection]);

    const CenterHeader = useCallback(({ centerId, centerName, anySelected, allSelected, testKeys, onRemove }) => (
        <div style={{ display: "flex", alignItems: "center", gap: "8px", marginBottom: "8px" }}>
            <input
                type="checkbox"
                checked={anySelected}
                onChange={() => handleCenterBulkSelection(testKeys, allSelected)}
                style={{ accentColor: "#4b3b8b", width: "clamp(5%, 30vw, 5%)", height: "clamp(5%, 30vw, 5%)", transform: "scale(1.2)", marginRight: "0" }}
            />
            <strong style={{ color: "#65636e", fontSize: "16px" }}>{centerName}</strong>
            <div style={{ marginLeft: "auto" }}>
                <CustomButton
                    type="button"
                    label="✖ Remove"
                    className="investigation-action-btn mt-2"
                    width="clamp(80px, 30vw, 80px)"
                    height="clamp(30px, 2.3vw, 30px)"
                    textColor="red"
                    backgroundColor="#FAF8FA"
                    borderRadius="3px"
                    shape="Square"
                    borderColor="1px solid red"
                    labelStyle={{ fontSize: "clamp(12px, 2vw, 13px)", fontWeight: "600", textTransform: "capitalize" }}
                    hoverEffect="theme"
                    onClick={onRemove}
                />
            </div>
        </div>
    ), [handleCenterBulkSelection]);

    // Common button style
    const commonButtonStyle = {
        width: "clamp(80px, 30vw, 80px)",
        height: "clamp(30px, 2.3vw, 30px)",
        textColor: "var(--theme-font-color)",
        backgroundColor: "#FAF8FA",
        borderRadius: "3px",
        shape: "Square",
        borderColor: "1px solid var(--theme-font-color)",
        labelStyle: { fontSize: "clamp(14px, 2vw, 16px)", fontWeight: "100", textTransform: "capitalize" },
        hoverEffect: "theme"
    };

    return (
        <CustomModal
            isOpen={isOpen}
            modalName={<span style={{ color: "#65636e", fontWeight: "1000", fontSize: "20px", fontFamily: "Georama" }}>Change Test Centers</span>}
            close={onClose}
            animationDirection="bottom"
            modalSize="medium"
            position="middle"
            closeOnOverlayClick={false}
            dataPreview
            form
        >
            <div className="fade-in">
                {/* Center Selection */}
                <div className="mb-3" style={{ textAlign: "left" }}>
                    <CustomSelect
                        label="Diagnostic Center"
                        labelPosition="top-left"
                        placeholder="Select a Test Center"
                        name="DiagnosticCenterName"
                        value={formData.DiagnosticCenterName}
                        onChange={handleSelectChange}
                        textColor="#65636e"
                        borderColors="1px solid #65636e"
                        width="100%"
                        options={testCenterOptions}
                    // disabled={isMaxCentersReached}
                    />
                </div>

                {/* Add Button */}
                <div className="w-100 d-flex justify-content-end">
                    <CustomButton
                        {...commonButtonStyle}
                        type="button"
                        label="Add"
                        className="investigation-action-btn mt-2"
                        onClick={handleAddCenter}
                    // disabled={isMaxCentersReached} 
                    />
                </div>

                {/* Info Message */}
                <div style={{ marginTop: "2px", textAlign: "left" }}>
                    <span style={{ width: "clamp(62%, 30vw, 62%)", textAlign: "left", fontWeight: "500", fontSize: "12px", color: "#e51e1eff" }}>
                        Only 2 test centers can be selected for free. <br />Redeem reward points to add more centers.
                    </span>

                    {/* Doctor Recommended Centers */}
                    {doctorRecommendedCentersData.map(({ centerId, centerName, filteredTests, testKeys, allSelected, anySelected }) => (
                        <div key={`doctor-${centerId}`} style={{ marginBottom: "20px", borderBottom: "1px solid #ddd" }}>
                            <CenterHeader centerId={centerId} centerName={centerName} anySelected={anySelected} allSelected={allSelected}
                                testKeys={testKeys} onRemove={() => handleRemoveDoctorRecommendedCenter(centerId)} />
                            <div style={{ marginLeft: "20px" }}>
                                {filteredTests.map(test => <TestItem key={`${centerId}_${test.testName}`} centerId={centerId} test={test} />)}
                            </div>
                        </div>
                    ))}

                    {/* Remove Error */}
                    {centerRemoveError && <div style={{ color: "red", fontSize: "14px", marginBottom: "10px", fontWeight: "500" }}>{centerRemoveError}</div>}

                    {/* User-added Centers */}
                    {addedCenters.map(({ center, tests }) => {
                        const centerId = center.testCenterId;
                        const testKeys = tests.map(test => `${centerId}_${test.testName}`);
                        const allSelected = testKeys.every(key => selectedTests[key]);
                        const anySelected = testKeys.some(key => selectedTests[key]);

                        return (
                            <div key={`added-${centerId}`} style={{ marginBottom: "20px" }}>
                                <CenterHeader centerId={centerId} centerName={center.testCenterName} anySelected={anySelected}
                                    allSelected={allSelected} testKeys={testKeys} onRemove={() => handleRemoveCenter(centerId)} />
                                <div style={{ marginLeft: "16px", marginTop: "8px" }}>
                                    {tests?.map((test, idx) => <TestItem key={idx} centerId={centerId} test={test} />)}
                                </div>
                            </div>
                        );
                    })}
                </div>

                {/* Submit Button */}
                <div className="w-100 d-flex justify-content-center gap-2 mt-3">
                    <CustomButton {...commonButtonStyle} type="button" label="Submit" className="investigation-action-btn mt-2"
                        onClick={handleSaveSelectedCenters} />
                </div>
            </div>
            <RewardPopup
                            isOpen={showRewardPopup}
                            onClose={() => {
                                setShowRewardPopup(false);
                                onClose();
                            }}
                            points={uploadResponse?.response?.totalRewardPoints || 0}
                            message={
                                "Congratulations! " +
                                (uploadResponse?.response?.rewardMessage || "")
                            }
            />
        </CustomModal>
    );
};

export default AddEditTestCenterModal;