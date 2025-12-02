import { useCallback, useEffect, useMemo, useRef, useState } from "react";
import axios from "axios";
import "./RxFilesFoldersList.css";
import FileList from "./FileList";
import FolderView from "./FolderView";
import PatientViewList from "./PatientViewList";
import PatientProfileMenu from "./PatientProfileMenu";
import DoctorProfileMenu from "./DoctorProfileMenu";
import { Breadcrumb } from "react-bootstrap";
import PageTitle from "../static/PageTitle/PageTitle";
import { useFetchData } from "../../hooks/useFetchData";
import CustomInput from "../static/Commons/CustomInput";
import { useFolder } from "../../contexts/FolderContext";
import useApiClients from "../../services/useApiClients";
import SearchIcon from "../../assets/img/SearchIcon.svg";
import { useParams, useNavigate, useLocation } from "react-router-dom";
import FolderManagementModal from "./FolderManagementModal";
import NormalViewToggle from "../static/Toggles/NormalViewToggle";
import useCurrentUserId from "../../hooks/useCurrentUserId";
import {
    findFolderById,
    renameFolderById,
    findFolderByPath,
} from "../../utils/utils";
import RxFolderShimmer from "./RxFolderShimmer";
import { VscSignOut } from "react-icons/vsc";
import { useLocalStorage } from "../../hooks/useLocalStorage";
import useApiServiceCall from "../../hooks/useApiServiceCall";

const RxFilesFoldersList = () => {
    const [search, setSearch] = useState("");
    const [modalType, setModalType] = useState(null);
    const [expandedIndex, setExpandedIndex] = useState(null);
    const [selectedFolder, setSelectedFolder] = useState(null);
    const [isPatientView, setIsPatientView] = useLocalStorage(
        "isPatientView",
        false,
    );
    const [isSwitchPatientEnabled, setIsSwitchPatientEnabled] = useState(false);
    const [isProfileMenuVisible, setIsProfileMenuVisible] = useState(false);
    const [isDoctorProfileMenuVisible, setIsDoctorProfileMenuVisible] =
        useState(false);
    const [currentFolder, setCurrentFolder] = useState(null);
    const [currentFolderInfo, setCurrentFolderInfo] = useState(null); // Track current folder info for navigation
    const [childrenData, setChildrenData] = useState(null); // Store children data from navigation
    const [apiChildrenItems, setApiChildrenItems] = useState([]); // Store full children list from API
    const [rootFolderId, setRootFolderId] = useState(null); // Track API root ("Primary") folder id to avoid routing into it
    const [refreshKey, setRefreshKey] = useState(0);

    const [currentPage, setCurrentPage] = useState(1);
    const [sortBy, setSortBy] = useState("createddateDesc");
    const [sortDirection, setSortDirection] = useState("desc");
    const [itemsPerPage, setItemsPerPage] = useState(10);
    const [debouncedSearch, setDebouncedSearch] = useState("");

    const [firstPageItems, setFirstPageItems] = useState([]);
    const [nextPageItems, setNextPageItems] = useState([]);
    const [folderAndFileData, setFolderAndFileData] = useState([]);
    const [combinedItems, setCombinedItems] = useState([]);
    const [visibleItemsState, setVisibleItemsState] = useState([]);
    const location = useLocation();
    const navigate = useNavigate();
    const { api } = useApiClients();
    const resolvedUserId = useCurrentUserId();
    const prevFolderIdRef = useRef(null);
    const { "*": currentPath, patientId, folderId } = useParams();
    const { setSelectedFolder: setContextSelectedFolder, setRefetch } =
        useFolder();

    const handleSearchChange = useCallback((input) => {
        const nextValue =
            typeof input === "string"
                ? input
                : (input?.target?.value ?? input?.value ?? "");
        setSearch(nextValue);
    }, []);

    const handleDataRefresh = useCallback(
        (movedFileId, actionType, renamedValue) => {
            const normalizeId = (value) => {
                const parsed = Number(value);
                return Number.isNaN(parsed) ? value : parsed;
            };

            const normalizedId =
                movedFileId == null ? null : normalizeId(movedFileId);

            const shouldPrune = normalizedId != null && actionType !== "rename";
            const shouldApplyRename =
                actionType === "rename" &&
                normalizedId != null &&
                typeof renamedValue === "string" &&
                renamedValue.trim().length > 0;
            const trimmedRename = shouldApplyRename
                ? renamedValue.trim()
                : undefined;

            const matchesTarget = (value) => {
                if (normalizedId == null) return false;
                if (value == null) return false;
                return normalizeId(value) === normalizedId;
            };

            const applyRenameToItem = (item) => {
                if (!item || !shouldApplyRename) return item;
                const targetMatches =
                    matchesTarget(item?.folderId) ||
                    matchesTarget(item?.FolderId) ||
                    matchesTarget(item?.id) ||
                    matchesTarget(item?.Id);
                if (!targetMatches) return item;
                return {
                    ...item,
                    folderOrFileName:
                        trimmedRename ??
                        item?.folderOrFileName ??
                        item?.folderName,
                    folderName:
                        trimmedRename ??
                        item?.folderName ??
                        item?.folderOrFileName,
                    name: trimmedRename ?? item?.name,
                };
            };

            const applyRenameToList = (list) => {
                if (!Array.isArray(list) || !shouldApplyRename) return list;
                let changed = false;
                const updated = list.map((item) => {
                    const nextItem = applyRenameToItem(item);
                    if (nextItem !== item) {
                        changed = true;
                    }
                    return nextItem;
                });
                return changed ? updated : list;
            };

            const matchesMovedFile = (item) => {
                if (!shouldPrune || !item) return false;
                const candidates = [
                    item?.fileId,
                    item?.PrescriptionId,
                    item?.prescriptionId,
                    item?.id,
                    item?.Id,
                ];
                return candidates.some(
                    (candidate) =>
                        candidate != null &&
                        normalizeId(candidate) === normalizedId,
                );
            };

            const pruneList = (list) => {
                if (!Array.isArray(list)) return list;
                if (!shouldPrune) return list;
                let changed = false;
                const filtered = list.filter((item) => {
                    const removeItem = matchesMovedFile(item);
                    if (removeItem) changed = true;
                    return !removeItem;
                });
                return changed ? filtered : list;
            };

            if (shouldApplyRename) {
                setFirstPageItems((prev) => applyRenameToList(prev));
                setNextPageItems((prev) => applyRenameToList(prev));
                setVisibleItemsState((prev) => applyRenameToList(prev));
                setApiChildrenItems((prev) => applyRenameToList(prev));
                setChildrenData((prev) => applyRenameToList(prev));
                setFolderAndFileData((prev) => {
                    if (!prev) return prev;
                    const prevChildrenData = prev?.children?.data;
                    const nextChildrenData =
                        applyRenameToList(prevChildrenData);
                    const childrenChanged =
                        nextChildrenData !== prevChildrenData;
                    const selfMatches = matchesTarget(prev?.folderId);
                    if (!childrenChanged && !selfMatches) {
                        return prev;
                    }
                    return {
                        ...prev,
                        ...(childrenChanged && prev?.children
                            ? {
                                  children: {
                                      ...prev.children,
                                      data: nextChildrenData,
                                  },
                              }
                            : {}),
                        ...(selfMatches
                            ? {
                                  folderOrFileName:
                                      trimmedRename ??
                                      prev?.folderOrFileName ??
                                      prev?.folderName,
                                  folderName:
                                      trimmedRename ??
                                      prev?.folderName ??
                                      prev?.folderOrFileName,
                              }
                            : {}),
                    };
                });
                if (matchesTarget(folderId)) {
                    setCurrentFolderInfo((prev) => {
                        if (!prev) return prev;
                        return {
                            ...prev,
                            folderOrFileName:
                                trimmedRename ??
                                prev?.folderOrFileName ??
                                prev?.folderName,
                        };
                    });
                    setCurrentFolder((prev) => {
                        if (!prev) return prev;
                        return {
                            ...prev,
                            folderOrFileName:
                                trimmedRename ??
                                prev?.folderOrFileName ??
                                prev?.folderName,
                            folderName:
                                trimmedRename ??
                                prev?.folderName ??
                                prev?.folderOrFileName,
                        };
                    });
                }
            }

            if (shouldPrune) {
                setFirstPageItems((prev) => pruneList(prev));
                setNextPageItems((prev) => pruneList(prev));
                setVisibleItemsState((prev) => pruneList(prev));
                setApiChildrenItems((prev) => pruneList(prev));
                setChildrenData((prev) => pruneList(prev));
                setFolderAndFileData((prev) => {
                    if (!prev) return prev;

                    const currentChildren = prev?.children?.data;
                    const currentPrescriptions = prev?.prescriptionList;

                    const updatedChildren = pruneList(currentChildren);
                    const updatedPrescriptions =
                        pruneList(currentPrescriptions);

                    const removedChildrenCount =
                        Array.isArray(currentChildren) &&
                        Array.isArray(updatedChildren)
                            ? currentChildren.length - updatedChildren.length
                            : 0;
                    const removedPrescriptionCount =
                        Array.isArray(currentPrescriptions) &&
                        Array.isArray(updatedPrescriptions)
                            ? currentPrescriptions.length -
                              updatedPrescriptions.length
                            : 0;

                    const childrenChanged =
                        Array.isArray(currentChildren) &&
                        updatedChildren !== currentChildren;
                    const prescriptionsChanged =
                        Array.isArray(currentPrescriptions) &&
                        updatedPrescriptions !== currentPrescriptions;

                    if (!childrenChanged && !prescriptionsChanged) {
                        return prev;
                    }

                    const nextTotalPrescriptions =
                        typeof prev.totalPrescriptionsCount === "number"
                            ? Math.max(
                                  0,
                                  prev.totalPrescriptionsCount -
                                      removedPrescriptionCount,
                              )
                            : prev.totalPrescriptionsCount;

                    return {
                        ...prev,
                        children: prev.children
                            ? {
                                  ...prev.children,
                                  data: updatedChildren,
                                  totalRecords:
                                      typeof prev.children.totalRecords ===
                                      "number"
                                          ? Math.max(
                                                0,
                                                prev.children.totalRecords -
                                                    removedChildrenCount,
                                            )
                                          : Array.isArray(updatedChildren)
                                            ? updatedChildren.length
                                            : prev.children.totalRecords,
                              }
                            : prev.children,
                        prescriptionList: updatedPrescriptions,
                        ...(typeof nextTotalPrescriptions !== "undefined" && {
                            totalPrescriptionsCount: nextTotalPrescriptions,
                        }),
                    };
                });
            }

            setRefreshKey((k) => k + 1);
        },
        [folderId],
    );

    useEffect(() => {
        if (typeof setRefetch === "function") {
            setRefetch(() => handleDataRefresh);
        }
    }, [handleDataRefresh, setRefetch]);
    // Force normal view on first load to avoid blank state from persisted Patient View
    useEffect(() => {
        setIsPatientView(false);
        setChildrenData(null);
        setCurrentFolderInfo(null);
    }, []);

    // UI display helper
    const getDisplayName = useCallback((name) => {
        if (!name) return name;
        return String(name).trim().toLowerCase() === "primary"
            ? "Rx Folder"
            : name;
    }, []);

    // Resolve a folder name by id using available data sources
    const findFolderNameInTree = useCallback((node, targetId) => {
        if (!node || typeof targetId !== "number") return null;
        if (node.folderId === targetId)
            return node.folderOrFileName || node.folderName || null;
        const childArr = node?.children?.data;
        if (Array.isArray(childArr)) {
            for (let i = 0; i < childArr.length; i++) {
                const name = findFolderNameInTree(childArr[i], targetId);
                if (name) return name;
            }
        }
        return null;
    }, []);

    const getFolderNameById = useCallback(
        (id) => {
            if (id == null) return null;
            // 1) Prefer parent info from navigation state, if present
            const stateParent = location?.state?.parentFolder;
            if (stateParent && stateParent.folderId === id) {
                return (
                    stateParent.folderOrFileName ||
                    stateParent.folderName ||
                    null
                );
            }
            // 2) Try global folders list if it contains items with folderId
            if (typeof folders !== "undefined" && Array.isArray(folders)) {
                const f = folders.find((f) => f?.folderId === id);
                if (f) return f.folderOrFileName || f.folderName || null;
            }
            // 3) Traverse current API tree
            const fromTree = findFolderNameInTree(folderAndFileData, id);
            if (fromTree) return fromTree;
            return null;
        },
        [folderAndFileData, location?.state, findFolderNameInTree],
    );

    // Helper functions for sorting
    const getSortField = (sortBy) => {
        switch (sortBy) {
            case "alphabeticAsc":
            case "alphabeticDesc":
                return "name";
            case "createddateAsc":
            case "createddateDesc":
                return "createdDate";
            default:
                return "createdDate";
        }
    };

    const getSortDirection = (sortBy) => {
        switch (sortBy) {
            case "alphabeticAsc":
            case "createddateAsc":
                return "asc";
            case "alphabeticDesc":
            case "createddateDesc":
                return "desc";
            default:
                return "desc";
        }
    };

    // Stable key extractor for deduplication across pages
    const getItemKey = useCallback((item) => {
        if (!item || typeof item !== "object") return null;
        // Folders
        const isFolder = item?.isFolder === true || item?.type === "folder";
        if (isFolder) {
            if (item?.folderId != null) return `folder-${item.folderId}`;
            const name =
                item?.folderOrFileName || item?.folderName || item?.name;
            const parent = item?.parentFolderId ?? "root";
            if (name)
                return `folder-name-${parent}-${String(name).toLowerCase()}`;
        }
        // Files
        const isFile = item?.isFolder === false || item?.type === "file";
        if (isFile) {
            if (item?.fileId != null) return `file-${item.fileId}`;
            const fname =
                item?.fileName || item?.folderOrFileName || item?.name;
            const created =
                item?.createdDateStr ||
                item?.createdDate ||
                item?.uploadedAt ||
                "";
            if (fname)
                return `file-name-${String(fname).toLowerCase()}-${String(created)}`;
        }
        // Prescriptions (Patient View)
        if (item?.type === "prescription") {
            if (item?.prescriptionId != null)
                return `rx-${item.prescriptionId}`;
            const owner = item?.patientId ?? item?.userId ?? "unknown";
            const pname = item?.prescriptionName || item?.patientName || "";
            const created = item?.createdDateStr || item?.createdDate || "";
            if (pname || created)
                return `rx-name-${owner}-${String(pname).toLowerCase()}-${String(created)}`;
        }
        // Generic fallback
        const genericId = item?.id ?? item?.Id;
        if (genericId != null) return `id-${genericId}`;
        return null;
    }, []);

    // Shallow identity check for first-page replacement to avoid update loops
    const listsEqualByKey = useCallback(
        (a = [], b = []) => {
            if (a.length !== b.length) return false;
            for (let i = 0; i < a.length; i++) {
                const ka = getItemKey(a[i]);
                const kb = getItemKey(b[i]);
                if (ka !== kb) return false;
                // If keys are null, fallback to simple pointer equality
                if (ka === null && a[i] !== b[i]) return false;
            }
            return true;
        },
        [getItemKey],
    );

    // Debounced search effect
    useEffect(() => {
        const timer = setTimeout(() => {
            setDebouncedSearch(search);
            setCurrentPage(1);
        }, 300);

        return () => clearTimeout(timer);
    }, [search]);

    // Create payload with the specified structure - use backend pagination
    const createPayload = () => {
        if (!resolvedUserId) {
            return null;
        }

        const payload = {
            UserId: resolvedUserId,
            PatientId: patientId ? Number(patientId) : null, // Use PatientId from URL params or default to 1
            FolderId: folderId ? Number(folderId) : null, // Use folderId from URL params when available
            FolderHeirarchy: currentFolder
                ? currentFolder.folderHeirarchy + 1
                : 0,
            ParentFolderId: currentFolder
                ? currentFolder.folderId
                : folderId
                  ? Number(folderId)
                  : null,
            CurrentFolderPath: currentPath || null,
            SearchKeyword: debouncedSearch?.trim() ? debouncedSearch : null,
            SearchColumn: null,
            PagingSorting: {
                PageNumber: currentPage,
                PageSize: itemsPerPage,
                SortBy: getSortField(sortBy),
                SortDirection: getSortDirection(sortBy),
            },
            ShowFoldersFirst: true,
        };
        return payload;
    };

    const {
        data: folders,
        isLoading: isFoldersLoading,
        error: allFoldersError,
        refetch: folderRefetch,
    } = useFetchData(
        api.getAllFoldersByUserId,
        null,
        null,
        null,
        null,
        resolvedUserId ?? undefined,
    );

    // Create payload for patient prescriptions
    const createPatientPrescriptionsPayload = () => {
        if (!resolvedUserId) {
            return null;
        }

        // Handle PatientId conversion more safely
        let processedPatientId = null;
        if (patientId) {
            const numPatientId = Number(patientId);
            if (!isNaN(numPatientId) && numPatientId > 0) {
                processedPatientId = numPatientId;
            }
        }

        return {
            UserId: resolvedUserId,
            // PatientId: processedPatientId, // Safely converted PatientId
            SearchKeyword: debouncedSearch || null,
            SearchColumn: null,
            PagingSorting: {
                PageNumber: currentPage, // Use actual current page
                PageSize: itemsPerPage, // Use actual items per page
                SortBy: getSortField(sortBy),
                SortDirection: getSortDirection(sortBy),
            },
        };
    };

    // Memoize payloads/params to avoid infinite loops from changing identities
    const browsePayload = useMemo(() => {
        const basePayload = createPayload();
        if (!basePayload) {
            return null;
        }

        // Add common backend aliases to maximize compatibility
        return {
            ...basePayload,
            _refreshKey: refreshKey,
        };
    }, [
        patientId,
        folderId,
        currentFolder,
        currentPath,
        debouncedSearch,
        sortBy,
        currentPage,
        itemsPerPage,
        refreshKey,
        resolvedUserId,
    ]);

    useEffect(() => {
        if (!browsePayload) {
            setFolderAndFileData(null);
            setApiChildrenItems([]);
            setVisibleItemsState([]);
            return;
        }

        let cancelled = false;
        (async () => {
            const response = await api.getBrowseRxFolderAndFileList(
                null,
                browsePayload,
            );
            if (cancelled) return;
            if (response?.message === "Successful") {
                const resp = response.response;
                console.log('API Response for BrowseRxFolderAndFileList:', resp);
                setFolderAndFileData(resp);
                // Capture root folder id when at root (parentFolderId is null)
                if (
                    resp &&
                    resp.parentFolderId == null &&
                    typeof resp.folderId === "number"
                ) {
                    setRootFolderId((prev) =>
                        prev !== resp.folderId ? resp.folderId : prev,
                    );
                }
                if (!isPatientView && !childrenData) {
                    const children = Array.isArray(resp?.children?.data)
                        ? resp.children.data
                        : [];
                    const files = Array.isArray(resp?.prescriptionList)
                        ? resp.prescriptionList
                        : [];
                    const childMapped = children.map((item, index) => ({
                        ...item,
                        type: item.isFolder ? "folder" : "file",
                        combinedIndex: index + 1,
                    }));
                    const fileMapped = files.map((file, fidx) => ({
                        ...file,
                        type: "file",
                        isFolder: false,
                        folderOrFileName:
                            file.folderOrFileName || file.fileName || file.name,
                        combinedIndex: childMapped.length + fidx + 1,
                    }));
                    const mapped = [...childMapped, ...fileMapped];
                    let nextAggregatedItems = mapped;
                    setApiChildrenItems((prev) => {
                        if (
                            currentPage === 1 ||
                            !Array.isArray(prev) ||
                            prev.length === 0
                        ) {
                            nextAggregatedItems = mapped;
                            return mapped;
                        }
                        const base = Array.isArray(prev) ? prev : [];
                        const seen = new Set(
                            base.map(getItemKey).filter(Boolean),
                        );
                        const appended = mapped.filter((item) => {
                            const key = getItemKey(item);
                            if (!key) {
                                return true;
                            }
                            if (seen.has(key)) {
                                return false;
                            }
                            seen.add(key);
                            return true;
                        });
                        nextAggregatedItems = appended.length
                            ? [...base, ...appended]
                            : base;
                        return nextAggregatedItems;
                    });
                    setVisibleItemsState(
                        Array.isArray(nextAggregatedItems)
                            ? nextAggregatedItems
                            : [],
                    );
                }
            } else {
                setFolderAndFileData(null);
                setCombinedItems([]);
            }
        })();
        return () => {
            cancelled = true;
        };
    }, [
        browsePayload,
        itemsPerPage,
        currentPage,
        sortBy,
        sortDirection,
        debouncedSearch,
        childrenData,
        getItemKey,
    ]); // stable, memoized

    useEffect(() => {
        setFirstPageItems([]);
        setNextPageItems([]);
        setVisibleItemsState([]);
    }, [refreshKey]);

    // Reset paginated/visible state when navigating between folders or to root
    useEffect(() => {
        setCurrentPage(1);
        setFirstPageItems([]);
        setNextPageItems([]);
        setVisibleItemsState([]);
        setApiChildrenItems([]);
        if (!folderId) {
            // Ensure we don't carry over previous folder context to root
            setCurrentFolder(null);
        }
    }, [folderId]);

    useEffect(() => {
        if (!folderId) {
            setCurrentFolderInfo((prev) => (prev !== null ? null : prev));
            return;
        }
        if (childrenData && Array.isArray(childrenData)) return;
        if (!folderAndFileData) return;
        const folderInfo = {
            folderId: Number(folderId),
            folderOrFileName: getDisplayName(
                folderAndFileData.folderOrFileName ||
                    folderAndFileData.folderName ||
                    `Folder ${folderId}`,
            ),
            createdDate: folderAndFileData.createdDate,
            createdDateStr: folderAndFileData.createdDateStr,
            parentFolderId: folderAndFileData.parentFolderId,
            folderHeirarchy: folderAndFileData.folderHeirarchy || 0,
            totalItems: folderAndFileData.children?.totalRecords || 0,
        };
        setCurrentFolderInfo((prev) => {
            if (
                prev &&
                prev.folderId === folderInfo.folderId &&
                prev.folderOrFileName === folderInfo.folderOrFileName &&
                prev.totalItems === folderInfo.totalItems
            )
                return prev;
            return folderInfo;
        });
    }, [
        folderId,
        folderAndFileData?.folderOrFileName,
        folderAndFileData?.children?.totalRecords,
        childrenData,
    ]);

    useEffect(() => {
        if (location.state && folderId) {
            const {
                folderId: stateFolderId,
                folderName,
                childrenData: stateChildrenData,
                parentFolder,
                parentFolderId: stateParentFolderId,
                selectedChild,
            } = location.state;

            if (stateChildrenData && Array.isArray(stateChildrenData)) {
                setChildrenData(stateChildrenData);
                const folderInfo = selectedChild
                    ? {
                          folderId: selectedChild.folderId,
                          folderOrFileName: getDisplayName(
                              selectedChild.folderOrFileName || folderName,
                          ),
                          createdDate: selectedChild.createdDate,
                          createdDateStr: selectedChild.createdDateStr,
                          parentFolderId: parentFolder?.folderId || null,
                          folderHeirarchy:
                              selectedChild.folderHeirarchy ??
                              (parentFolder?.folderHeirarchy || 0) + 1,
                          totalItems: stateChildrenData.length,
                      }
                    : {
                          folderId: stateFolderId,
                          folderOrFileName: getDisplayName(folderName),
                          createdDate: parentFolder?.createdDate,
                          createdDateStr: parentFolder?.createdDateStr,
                          parentFolderId: parentFolder?.parentFolderId
                              ? parentFolder.parentFolderId
                              : null,
                          folderHeirarchy:
                              (parentFolder?.folderHeirarchy || 0) + 1,
                          totalItems: stateChildrenData.length,
                      };
                setCurrentFolderInfo(folderInfo);
            }
        } else {
            setChildrenData(null);
        }
    }, [location.state, folderId]);

    const hasValidPatientId =
        patientId && !isNaN(Number(patientId)) && Number(patientId) > 0;
    const shouldCallPatientPrescriptions =
        isPatientView && Boolean(resolvedUserId);

    // Patient prescriptions API call
    const {
        data: patientPrescriptionsData = {},
        isLoading: isPatientPrescriptionsLoading,
        error: patientPrescriptionsError,
        refetch: patientPrescriptionsRefetch,
    } = useFetchData(
        shouldCallPatientPrescriptions ? api.getPatientPrescriptions : null,
        shouldCallPatientPrescriptions ? currentPage : null, // Use actual current page
        shouldCallPatientPrescriptions ? itemsPerPage : null, // Use actual items per page
        shouldCallPatientPrescriptions ? getSortField(sortBy) : null,
        shouldCallPatientPrescriptions ? getSortDirection(sortBy) : null,
        shouldCallPatientPrescriptions
            ? { SearchParams: createPatientPrescriptionsPayload() }
            : null,
    );

    // Total available count from backend pagination metadata or children data
    const totalAvailableCount = useMemo(() => {
        if (isPatientView) {
            return Number(patientPrescriptionsData?.totalRecords ?? 0) || 0;
        } else {
            if (childrenData && Array.isArray(childrenData)) {
                return childrenData.length;
            } else {
                const childCount =
                    Number(folderAndFileData?.children?.totalRecords ?? 0) || 0;
                const fileCount = Array.isArray(
                    folderAndFileData?.prescriptionList,
                )
                    ? folderAndFileData.prescriptionList.length
                    : 0;
                return childCount + fileCount;
            }
        }
    }, [
        isPatientView,
        childrenData,
        folderAndFileData?.children?.totalRecords,
        patientPrescriptionsData?.totalRecords,
    ]);

    const totalPages = Math.max(
        1,
        Math.ceil(totalAvailableCount / itemsPerPage),
    );
    const canSeeMore = visibleItemsState?.length < totalAvailableCount;
    const handleSeeMore = () => {
        if (!canSeeMore) {
            return;
        }
        // Immediately reflect all currently accumulated items before fetching next page
        setVisibleItemsState((prev) => {
            const combined = [...firstPageItems, ...nextPageItems];
            return combined.length ? combined : prev;
        });
        // Trigger next page; accumulated results will be appended in api response handler
        setCurrentPage((p) => {
            const next = p + 1;
            const maxPage = Math.max(
                1,
                Math.ceil(totalAvailableCount / itemsPerPage),
            );
            return Math.min(next, maxPage);
        });
    };

    useEffect(() => {
        // Keep context in sync with current payload's folder if available
        const folderId = folderAndFileData?.folderId;
        if (folderId && folderId !== prevFolderIdRef.current) {
            setContextSelectedFolder(folderAndFileData);
            prevFolderIdRef.current = folderId;
        }
    }, [folderAndFileData, setContextSelectedFolder]);

    const openModal = (type) => {
        setModalType(type); // Open the modal and set the type
    };

    const closeModal = () => {
        setModalType(null); // Close the modal
    };

    const allCombinedItems = useMemo(() => {
        if (isPatientView) {
            // Patient view: show patient prescriptions from backend API
            const prescriptions = Array.isArray(patientPrescriptionsData?.data)
                ? patientPrescriptionsData.data
                : [];

            // Apply search filter to prescriptions if search is active
            let filteredPrescriptions = prescriptions;
            if (debouncedSearch && debouncedSearch.trim()) {
                const searchTerm = debouncedSearch.toLowerCase().trim();
                filteredPrescriptions = prescriptions.filter((prescription) => {
                    const name = (
                        prescription?.patientName ||
                        prescription?.prescriptionName ||
                        ""
                    ).toLowerCase();
                    return name.includes(searchTerm);
                });
            }

            // Sort prescriptions by selected sort
            const sortedPrescriptions = [...filteredPrescriptions].sort(
                (a, b) => {
                    const parseDate = (raw) => {
                        if (!raw) return 0;
                        const ts = Date.parse(raw);
                        return Number.isNaN(ts) ? 0 : ts;
                    };
                    const itemDate = (item) =>
                        parseDate(item?.createdDate ?? item?.createdDateStr);
                    const itemName = (item) =>
                        (
                            item?.patientName ||
                            item?.prescriptionName ||
                            ""
                        )?.toLowerCase();

                    if (
                        sortBy === "alphabeticAsc" ||
                        sortBy === "alphabeticDesc"
                    ) {
                        const comparison = itemName(a).localeCompare(
                            itemName(b),
                        );
                        return sortBy === "alphabeticDesc"
                            ? -comparison
                            : comparison;
                    } else if (
                        sortBy === "createddateAsc" ||
                        sortBy === "createddateDesc"
                    ) {
                        const dateA = itemDate(a);
                        const dateB = itemDate(b);
                        return sortBy === "createddateAsc"
                            ? dateA - dateB
                            : dateB - dateA;
                    }
                    const dateA = itemDate(a);
                    const dateB = itemDate(b);
                    return dateB - dateA;
                },
            );

            return sortedPrescriptions.map((prescription, index) => ({
                ...prescription,
                type: "prescription",
                combinedIndex: index + 1,
            }));
        }

        if (childrenData && Array.isArray(childrenData) && !isPatientView) {
            // Use navigation-provided data, preserve original order; filter only
            let filteredData = childrenData;
            if (debouncedSearch && debouncedSearch.trim()) {
                const searchTerm = debouncedSearch.toLowerCase().trim();
                filteredData = childrenData.filter((item) => {
                    const name = (
                        item?.folderOrFileName ||
                        item?.fileName ||
                        ""
                    ).toLowerCase();
                    return name.includes(searchTerm);
                });
            }

            return filteredData.map((item, index) => ({
                ...item,
                type: item.isFolder ? "folder" : "file",
                combinedIndex: index + 1,
            }));
        }

        // Use backend data (children.data) and preserve API order (folders first, then files)
        const allItemsRaw = Array.isArray(apiChildrenItems)
            ? apiChildrenItems
            : [];

        // Optional search without reordering
        let filtered = allItemsRaw;
        if (debouncedSearch && debouncedSearch.trim()) {
            const searchTerm = debouncedSearch.toLowerCase().trim();
            filtered = allItemsRaw.filter((item) => {
                const name = (
                    item?.folderOrFileName ||
                    item?.fileName ||
                    ""
                ).toLowerCase();
                return name.includes(searchTerm);
            });
        }
        return filtered.map((item, index) => ({
            ...item,
            type: item.isFolder ? "folder" : "file",
            combinedIndex: index + 1,
        }));
    }, [
        isPatientView,
        childrenData,
        apiChildrenItems,
        patientPrescriptionsData?.data,
        sortBy,
        debouncedSearch,
    ]);

    useEffect(() => {
        if (isPatientView) {
            setVisibleItemsState((prev) =>
                listsEqualByKey(prev, allCombinedItems)
                    ? prev
                    : allCombinedItems,
            );
            return;
        }

        if (childrenData && Array.isArray(childrenData) && !isPatientView) {
            setVisibleItemsState((prev) =>
                listsEqualByKey(prev, allCombinedItems)
                    ? prev
                    : allCombinedItems,
            );
            return;
        }

        setVisibleItemsState((prev) =>
            listsEqualByKey(prev, allCombinedItems) ? prev : allCombinedItems,
        );
    }, [isPatientView, childrenData, allCombinedItems, listsEqualByKey]);

    useEffect(() => {
        // Backend pagination accumulation
        if (!childrenData) {
            if (currentPage === 1) {
                // Only replace if actually changed to avoid loops
                setFirstPageItems((prev) => {
                    if (prev.length === allCombinedItems.length) {
                        let same = true;
                        for (let i = 0; i < prev.length; i++) {
                            const ka = getItemKey(prev[i]);
                            const kb = getItemKey(allCombinedItems[i]);
                            if (ka !== kb) {
                                same = false;
                                break;
                            }
                        }
                        if (same) return prev;
                    }
                    return allCombinedItems;
                });
                // Clear next page items only if non-empty
                setNextPageItems((prev) => (prev.length ? [] : prev));
            } else if (allCombinedItems.length > 0) {
                // Append only new items to nextPageItems using stable keys
                setNextPageItems((prev) => {
                    // Avoid deduping unless we have reliable IDs
                    const hasReliableIds = allCombinedItems.every(
                        (i) => getItemKey(i) !== null,
                    );
                    if (!hasReliableIds) {
                        return [...prev, ...allCombinedItems];
                    }
                    const existingKeys = new Set([
                        ...firstPageItems.map(getItemKey).filter(Boolean),
                        ...prev.map(getItemKey).filter(Boolean),
                    ]);
                    const newItems = allCombinedItems.filter((item) => {
                        const key = getItemKey(item);
                        return key ? !existingKeys.has(key) : true;
                    });
                    if (newItems.length === 0) return prev;
                    return [...prev, ...newItems];
                });
            }
        } else {
            // Using childrenData (client-side pagination): no accumulation; handled via slicing below
            setFirstPageItems((prev) => (prev.length ? [] : prev));
            setNextPageItems((prev) => (prev.length ? [] : prev));
        }
    }, [
        allCombinedItems,
        currentPage,
        childrenData,
        getItemKey,
        firstPageItems,
    ]);

    // First toggle handler for Patient View vs Normal View
    const handleViewToggle = (e) => {
        const isPatientViewEnabled = e.target.checked;
        setIsPatientView(isPatientViewEnabled);
        setCurrentPage(1); // Reset to first page when switching views
        setChildrenData(null); // Clear children data when switching views
        setCurrentFolderInfo(null); // Clear folder info when switching views
        setFirstPageItems([]);
        setNextPageItems([]);
        setVisibleItemsState([]);

        // Reset search when switching views
        setSearch("");
        setDebouncedSearch("");

        // If switching to Patient View and currently in a folder, navigate to root
        if (isPatientViewEnabled && (folderId || currentFolderInfo)) {
            navigate("/browserx");
        }
    };

    const handleFolderNavigation = useCallback(
        (folder) => {
            if (!folder || folder.folderId == null) {
                console.warn("Unable to navigate: missing folderId", folder);
                return;
            }

            setSearch("");
            setDebouncedSearch("");
            setCurrentPage(1);
            setChildrenData(null);
            setCurrentFolder(null);
            setCurrentFolderInfo(null);
            setFirstPageItems([]);
            setNextPageItems([]);
            setVisibleItemsState([]);
            setApiChildrenItems([]);

            navigate(`/browserx/folder/${folder.folderId}`, {
                state: {
                    folderId: folder.folderId,
                    folderName:
                        folder.folderOrFileName ||
                        folder.folderName ||
                        getDisplayName(folder.folderOrFileName),
                    selectedChild: folder,
                    parentFolder:
                        folder.parentFolder ??
                        (folder.parentFolderId
                            ? { folderId: folder.parentFolderId }
                            : null),
                },
            });
        },
        [
            navigate,
            setChildrenData,
            setCurrentFolder,
            setCurrentFolderInfo,
            setDebouncedSearch,
            getDisplayName,
        ],
    );

    // Profile menu handlers
    const handleCloseDoctorProfileMenu = () => {
        setIsDoctorProfileMenuVisible(false);
    };
    const handleRenameClick = (folder) => {
        setSelectedFolder(folder);
        openModal("rename");
    };

    const handleDeleteClick = (folder) => {
        try {
            setSelectedFolder(folder);
            openModal("delete");
        } catch (error) {
            console.error("Error opening delete modal:", error);
            alert(
                `Error opening delete modal: ${error.message || "Unknown error occurred"}`,
            );
        }
    };

    return isFoldersLoading || isPatientPrescriptionsLoading ? (
        <RxFolderShimmer />
    ) : (
        <div className="content-container">
            <div className="rx-folder-container row px-3 px-md-5">
                <div className="col-12 col-md-9 col-lg-7 col-xl-6 mx-auto p-0">
                    <PageTitle
                        pageName={
                            isPatientView
                                ? "Rx Folder"
                                : currentFolderInfo
                                  ? getDisplayName(
                                        currentFolderInfo.folderOrFileName,
                                    )
                                  : folderId
                                    ? `Folder (ID: ${folderId})`
                                    : currentFolder
                                      ? getDisplayName(
                                            currentFolder.folderOrFileName,
                                        )
                                      : "Rx Folder"
                        }
                    />

                    <div className="d-flex justify-content-between align-items-center">
                        <CustomInput
                            className={"w-100"}
                            rightIcon={SearchIcon}
                            name="search"
                            type="text"
                            placeholder="Search"
                            value={search}
                            onChange={handleSearchChange}
                            minHeight="0px"
                        />
                        <div className="d-flex align-items-center gap-2">
                            <NormalViewToggle
                                isPatientView={isPatientView}
                                onToggle={handleViewToggle}
                            />
                        </div>
                    </div>

                    <div className="mt-3 h-100 overflow-auto">
                        {/** Ensure visibleItems is always a safe array to avoid length errors */}
                        {(() => {
                            return null;
                        })()}
                        {isPatientView ? (
                            Array.isArray(visibleItemsState) &&
                            visibleItemsState.length > 0 ? (
                                visibleItemsState.map((prescription, index) => (
                                    <PatientViewList
                                        key={`prescription-${prescription.prescriptionId ?? prescription.patientId ?? prescription.combinedIndex ?? index}`}
                                        patient={prescription}
                                        index={index}
                                    />
                                ))
                            ) : (
                                <div className="text-center mt-5 text-muted">
                                    {hasValidPatientId
                                        ? "No prescriptions found"
                                        : "No prescriptions found"}
                                </div>
                            )
                        ) : Array.isArray(visibleItemsState) &&
                          visibleItemsState.length > 0 ? (
                            (visibleItemsState || []).map((child, index) => {
                                if (child.type === "folder") {
                                    return (
                                        <FolderView
                                            key={`folder-${child.folderId ?? child.folderOrFileName ?? child.combinedIndex ?? index}-${index}`}
                                            item={child}
                                            index={index}
                                            expandedIndex={expandedIndex}
                                            setExpandedIndex={setExpandedIndex}
                                            onClick={() =>
                                                handleFolderNavigation(child)
                                            }
                                            onRenameClick={handleRenameClick}
                                            onDeleteClick={handleDeleteClick}
                                            foldersList={folders}
                                        />
                                    );
                                } else if (child.type === "file") {
                                    return (
                                        <FileList
                                            key={`file-${child.fileId ?? child.fileName ?? child.combinedIndex ?? index}-${index}`}
                                            item={child}
                                            index={index}
                                            expandedIndex={expandedIndex}
                                            setExpandedIndex={setExpandedIndex}
                                            refetch={handleDataRefresh}
                                            foldersList={folders}
                                            totalPrescriptions={
                                                child.totalPrescriptions
                                            }
                                        />
                                    );
                                }
                                return null;
                            })
                        ) : (
                            <div
                                className="text-center mt-5 text-muted"
                                style={{
                                    fontFamily: "Georama",
                                    color: "#65636e",
                                }}
                            >
                                This folder is empty
                            </div>
                        )}
                        {Array.isArray(visibleItemsState) &&
                            visibleItemsState.length > 0 && (
                                <div className="mt-4">
                                    <div className="d-flex justify-content-center align-items-center mb-2">
                                        <div
                                            className="text-muted"
                                            style={{
                                                fontFamily: "Georama",
                                                color: "#65636e",
                                            }}
                                        >
                                            {`Showing ${visibleItemsState.length} of ${totalAvailableCount} items (Page ${currentPage} up to ${totalPages})`}
                                        </div>
                                    </div>
                                    {canSeeMore && (
                                        <div className="d-flex justify-content-center">
                                            <button
                                                className="see-more-button"
                                                onClick={handleSeeMore}
                                            >
                                                See more ...
                                            </button>
                                        </div>
                                    )}
                                </div>
                            )}
                    </div>
                    {/* See More pattern */}
                </div>
            </div>
            <FolderManagementModal
                modalType={modalType}
                isOpen={!!modalType}
                folderData={selectedFolder}
                onClose={closeModal}
                fetchFolders={(fileId, actionType, renamedValue) => {
                    try {
                        handleDataRefresh(fileId, actionType, renamedValue);
                    } catch (error) {
                        console.error("Error refreshing data:", error);
                        alert(
                            `Error refreshing data: ${error.message || "Unknown error occurred"}`,
                        );
                    }
                }}
                folderRefetch={() => {
                    try {
                        folderRefetch();
                        handleDataRefresh();
                    } catch (error) {
                        console.error("Error refetching folders:", error);
                        alert(
                            `Error refreshing folders: ${error.message || "Unknown error occurred"}`,
                        );
                    }
                }}
            />

            {/* Doctor Profile Menu */}
            <DoctorProfileMenu
                isVisible={isDoctorProfileMenuVisible}
                onClose={handleCloseDoctorProfileMenu}
            />
        </div>
    );
};

export default RxFilesFoldersList;
