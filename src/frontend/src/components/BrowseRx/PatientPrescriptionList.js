import React, { useEffect, useState, useMemo } from 'react'
import { useLocation, useNavigate } from "react-router-dom";
import PageTitle from '../static/PageTitle/PageTitle';
import SearchIcon from "../../assets/img/SearchIcon.svg";
import CustomInput from '../static/Commons/CustomInput';
import FileList from './FileList';
import useApiClients from '../../services/useApiClients';
import useCurrentUserId from '../../hooks/useCurrentUserId';
import { useFetchData } from '../../hooks/useFetchData';
import { filterData } from '../../utils/tableUtils';

const PatientPrescriptionList = () => {
    const [search, setSearch] = useState("");
    const [currentPage, setCurrentPage] = useState(1);
    const [sortBy, setSortBy] = useState("name");
    const [sortDirection, setSortDirection] = useState("asc");
    const [itemsPerPage, setItemsPerPage] = useState(10);
    const [debouncedSearch, setDebouncedSearch] = useState("");
    const [expandedIndex, setExpandedIndex] = useState(null);
    const [ownerName, setOwnerName] = useState("");

    const { state } = useLocation();
    const { api } = useApiClients();
    const loginUserId = useCurrentUserId();

    const patientId = useMemo(() => {
        const raw = state?.patientId ?? state?.PatientId;
        if (raw === undefined || raw === null) {
            return null;
        }
        const parsed = Number(raw);
        return Number.isFinite(parsed) && parsed > 0 ? parsed : null;
    }, [state]);

    // state?.prescriptionType;

    const payload = useMemo(() => {
        if (!loginUserId) {
            return null;
        }

        return {
            UserId: loginUserId,
            PatientId: patientId,
            PrescriptionType: state?.prescriptionType,
            PagingSorting: {
                PageNumber: currentPage,
                PageSize: itemsPerPage,
                SortBy: sortBy,
                SortDirection: sortDirection,
            },
        };
    }, [loginUserId, patientId, state?.prescriptionType, currentPage, itemsPerPage, sortBy, sortDirection]);
    const getSortField = (sortBy) => {
        // if (sortBy === "lowToHigh" || sortBy === "highToLow") return "rating";
        if (sortBy === "yearAsc" || sortBy === "yearDesc") return "createdDate";
        if (sortBy === "alphabeticAsc" || sortBy === "alphabeticDesc") return "name";
        return "name";
    };

    const getSortDirection = (sortBy) => {
        if (
            // sortBy === "lowToHigh" ||
            sortBy === "yearAsc" ||
            sortBy === "alphabeticAsc"
        )
            return "asc";
        if (
            // sortBy === "highToLow" ||
            sortBy === "yearDesc" ||
            sortBy === "alphabeticDesc"
        )
            return "desc";
        return "asc";
    };
    const {
        data: patientPrescriptionsData,
        isLoading: isPatientPrescriptionsLoading,
        error: patientPrescriptionsError,
        refetch,
    } = useFetchData(
        loginUserId ? api.getPatientPrescriptionsByType : null,
        loginUserId ? currentPage - 1 : null,
        loginUserId ? itemsPerPage : null,
        loginUserId ? getSortField(sortBy) : null,
        loginUserId ? getSortDirection(sortBy) : null,
        payload,
        loginUserId ?? undefined,
    );
    // Resolve owner name with fallbacks
    useEffect(() => {
        if (state?.prescriptionOwner) {
            setOwnerName(state.prescriptionOwner);
            return;
        }

        // Try from state fields if available
        const stateFirstName = state?.firstName || state?.patientFirstName;
        const stateLastName = state?.lastName || state?.patientLastName;
        const nameFromState = `${stateFirstName || ""} ${stateLastName || ""}`.trim();
        if (nameFromState) {
            setOwnerName(nameFromState);
            return;
        }

        // If prescriptions data contains patient info, infer from first item
        const firstItem = patientPrescriptionsData?.data?.[0];
        const dataFirstName = firstItem?.patientFirstName || firstItem?.firstName;
        const dataLastName = firstItem?.patientLastName || firstItem?.lastName;
        const nameFromData = `${dataFirstName || ""} ${dataLastName || ""}`.trim();
        if (nameFromData) {
            setOwnerName(nameFromData);
            return;
        }

        // As a final fallback, fetch patient detail if patientId is present
        const fetchOwnerByPatientId = async () => {
            try {
                if (state?.patientId) {
                    const resp = await api.getPatientDataById({ patientId: state.patientId });
                    const p = resp?.response?.data || resp?.response || resp?.data;
                    const f = p?.firstName || p?.patientFirstName;
                    const l = p?.lastName || p?.patientLastName;
                    const name = `${f || ""} ${l || ""}`.trim();
                    if (name) setOwnerName(name);
                }
            } catch (e) {
                // Silent fail; title will fallback
            }
        };
        fetchOwnerByPatientId();
    }, [state, patientPrescriptionsData]);

    // Debounced search effect
    useEffect(() => {
        const timer = setTimeout(() => {
            setDebouncedSearch(search);
            setCurrentPage(1);
        }, 300);

        return () => clearTimeout(timer);
    }, [search]);

    // Filter prescriptions data based on search query
    const filteredPrescriptions = useMemo(() => {
        if (!patientPrescriptionsData?.data || !debouncedSearch.trim()) {
            return patientPrescriptionsData?.data || [];
        }
        return filterData(patientPrescriptionsData.data, debouncedSearch);
    }, [patientPrescriptionsData?.data, debouncedSearch]);

    return (
        <div className="content-container">
            <div className="rx-folder-container row px-3 px-md-5">
                <div className="col-12 col-md-9 col-lg-7 col-xl-6 mx-auto p-0">
                    <PageTitle
                        pageName={
                            state?.prescriptionType === "smartrx"
                                ? `${(ownerName || "Patient").split(" ")[0]}'s SmartRx List`
                                : state?.prescriptionType === "waiting"
                                ? "Waiting List"
                                : "Uncategorized List"
                        }
                        smallText={state?.prescriptionType === "smartrx"}
                    />
                    <div className="d-flex justify-content-between align-items-center">
                        <CustomInput
                            className={"w-100"}
                            rightIcon={SearchIcon}
                            name="search"
                            type="text"
                            placeholder="Search"
                            value={search}
                            onChange={(e) => setSearch(e.target.value)}
                            minHeight="0px"
                        />
                    </div>
                    <div className="mt-3 h-100 overflow-auto">
                        {filteredPrescriptions?.length > 0 ? (
                            filteredPrescriptions.map((item, index) => (
                                <FileList
                                    key={`file-${item.fileId ?? item.fileName ?? item.combinedIndex ?? index}`}
                                    item={item}
                                    index={index}
                                    expandedIndex={expandedIndex}
                                    setExpandedIndex={setExpandedIndex}
                                    refetch={refetch}
                                    foldersList={[]}
                                />
                            ))
                        ) : (
                            <div className="text-center mt-5 text-muted">
                                {debouncedSearch.trim() ? 
                                    `No prescriptions found matching "${debouncedSearch}"` : 
                                    "No prescriptions found"
                                }
                            </div>
                        )}

                    </div>
                </div>
            </div>
        </div>
    )
}

export default PatientPrescriptionList
