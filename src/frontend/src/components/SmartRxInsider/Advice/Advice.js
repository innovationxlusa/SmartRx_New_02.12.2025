import "./Advice.css";
import { ReactComponent as PlusIcon } from "../../../assets/img/Plus.svg";
import { ReactComponent as ThermometerIcon } from "../../../assets/img/ColdTherapy.svg";
import { ReactComponent as ToothIcon } from "../../../assets/img/ToothIcon.svg";
import AdviceManagementModal from "./AdviceManagementModal";
import { useState } from "react";
import { FaRegEdit, FaTrashAlt } from "react-icons/fa";
import { useNavigate } from "react-router-dom";
import CustomButton from "../../static/Commons/CustomButton";

const Advice = ({ smartRxInsiderData }) => {
    /* ───────────────────────
           ───── Local state ─────
           ─────────────────────── */
    const [modalType, setModalType] = useState(null);
    const [selectedAdvice, setSelectedAdvice] = useState(null);

    /* ───────── Modal helpers ───────── */
    const openModal = (type) => setModalType(type);
    const closeModal = () => setModalType(null);

    const handleOpenModal = (advice, type) => {
        setSelectedAdvice(advice);
        openModal(type);
    };

    //Dummy Data
    const dummyAdviceRecommendations = [
      {
        id: 1,
        icon: <ThermometerIcon className="advice-card-img" />,
        question: "Cold Therapy",
      },
      {
        id: 2,
        icon: <ToothIcon className="advice-card-img" />,
        question: "Dental Health",
      },
    ];



    const navigate = useNavigate();

    return (
      <div>
        {/* Doctor Recommendations */}
        <div className="d-flex justify-content-center flex-column align-items-center">
          <div className="advice-doctor-section">
            <PlusIcon className="advice-icon" />
            <div>
              <div className="advice-section-title">
                <span>Doctor Recommendation</span>
              </div>
              <ul className="advice-recommendation-list">
                {smartRxInsiderData?.prescriptions?.[0]?.advices?.map(
                  (advice, index) => {
                    return (
                      <li key={index}>
                        + <em> {advice.advice}</em>
                        {/* <FaRegEdit role="button" className="mx-2 curser" onClick={() => handleOpenModal(advice, "edit")} />
                                        <FaTrashAlt role="button" onClick={() => handleOpenModal(advice, "delete")} /> */}
                      </li>
                    );
                  }
                )}
              </ul>
            </div>
          </div>
          <CustomButton
            type="button"
            label="Add More"
            className="investigation-action-btn mt-2"
            width="clamp(250px, 30vw, 450px)"
            height="clamp(42px, 2.3vw, 40px)"
            textColor="var(--theme-font-color)"
            backgroundColor="#FAF8FA"
            borderRadius="3px"
            shape="Square"
            borderColor="1px solid var(--theme-font-color)"
            labelStyle={{
              fontSize: "clamp(14px, 2vw, 16px)",
              fontWeight: "100",
              textTransform: "capitalize",
            }}
            hoverEffect="theme"
            onClick={(e) => {
              e.stopPropagation();
              e.nativeEvent.stopImmediatePropagation();
              handleOpenModal(null, "add");
            }}
          />
        </div>

        {/* Therapy Cards */}
        <div className="therapy-cards">
          {dummyAdviceRecommendations.map((adviceRecommendation) => (
            <div key={adviceRecommendation.id} className="advice-card">
              {adviceRecommendation.icon}

              <div className="advice-card-content text-end">
                <h6>{adviceRecommendation.question}</h6>
                <PlusIcon className="advice-icon" />
                <button
                  className="advice-learn-btn"
                  onClick={(e) => {
                    e.stopPropagation();
                    e.nativeEvent.stopImmediatePropagation();
                    navigate("/blog", {
                      state: { question: adviceRecommendation.question },
                    });
                  }}
                >
                  Learn More
                </button>
              </div>
            </div>
          ))}
        </div>

        
        {/* <div className="therapy-cards">
                {smartRxInsiderData?.prescriptions?.[0]?.adviceRecommendations?.map(
                    (adviceRecommendation) => (
                        <div
                            key={adviceRecommendation.id}
                            className="advice-card"
                        >
                            <ThermometerIcon className="advice-card-img" />
                            <img
                                src={adviceRecommendation?.iconFilePath}
                                alt={adviceRecommendation?.iconFileName}
                                className="advice-card-img"
                            />

                            <div className="advice-card-content text-end">
                                <h6>{adviceRecommendation?.question}</h6>
                                <PlusIcon className="advice-icon" />
                                <button
                                    className="advice-learn-btn"
                                    onClick={(e) => {
                                        e.stopPropagation();
                                        e.nativeEvent.stopImmediatePropagation();
                                        navigate("/blog");
                                    }}
                                >
                                    Learn More
                                </button>
                            </div>
                        </div>
                    )
                )}
            </div> */}


        {/* ───── Add / Edit / Delete modal ───── */}
        <AdviceManagementModal
          modalType={modalType}
          isOpen={!!modalType}
          adviceData={selectedAdvice}
          onClose={closeModal}
          // fetchSmartRxVitalData={fetchSmartRxVitalData}
          anotherButton={"true"}
          // smartRxMasterId={smartRxMasterId}
          // prescriptionId={smartRxInsiderData?.prescriptions?.[0]?.prescriptionId}
          //refetch={refetch}
        />
      </div>
    );
};

export default Advice;
