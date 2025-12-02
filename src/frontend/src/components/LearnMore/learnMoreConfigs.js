import logo from "../../assets/img/RxLogo.png";
import logo1 from "../../assets/img/RxLogo1.png";
import { FiCamera, FiUpload } from "react-icons/fi";
import { ReactComponent as FolderIcon } from "../../assets/img/Folder.svg";
import { clamp } from "framer-motion";

export const learnMoreConfigs = {
    fileRx: {
        title: "FileRx Archive",
        subtitle: "Organize your family Prescription",
        videoSrc: "https://www.youtube.com/embed/dQw4w9WgXcQ",
        bannerText: "RX File to take care of Your Prescription File",
        mainContent: (
            <>
                <div className="content-head-div pt-3 pb-2 pt-md-5 pt-lg-5">
                    <div>
                        <FolderIcon className="folderIcon pb-0 pb-md-1" alt="Folder Icon" height={40} />
                    </div>
                    <p className="content-head mb-0"><b>FileRx</b></p>

                    <p className="content-head-text mb-0 mb-md-1 padding-bottom"> – Your Prescription Archive</p>
                </div>
                <div className="content-mid-text ps-3 ps-lg-3">Never lose a prescription again!</div>
                <div className="content-description ps-3 ps-lg-3">
                    You can upload unlimited prescription here in two (02) formats- image and pdf. Way to upload in the system are-
                    <div className="content-desc-icons ps-2 ps-md-5 ps-lg-5 pt-2 pb-2 gap-5">
                        <div className="upload-icon d-flex flex-column justify-content-center align-items-center">
                            <div className="upload-circle">
                                <FiUpload />
                            </div>
                            <div className="">File Upload</div>
                        </div>
                        <div className="upload-icon d-flex flex-column justify-content-center align-items-center">
                            <div className="upload-circle">
                                <FiCamera />
                            </div>
                            <div className="">Camera Capture</div>
                        </div>
                    </div>
                    On both option you can retake image or upload again if not seems good the file. After uploading your file, your goal is to Request for SmartRx. Also you can manipulate your file by
                    moving file folder to folder or move to main folder, preview, download, delete.
                    <ul className="bullet-list">
                        <li> Store all prescriptions securely. Once you stored your prescription here, it is the liability for us.</li>
                        <li> Create folders for each family members and separately manipulate your files for every person.</li>
                        <li> Manage your uploaded files. Access, move, view & print anytime, anywhere</li>
                        <li> Share files & profiles with others easily</li>
                    </ul>
                    <div className="d-flex align-items-start align-items-md-end">
                        <FolderIcon className="folderIcon me-2" alt="Folder Icon" width={20} height={20} />

                        <span>Easy, transparent, reliable place for your health records.</span>
                    </div>
                </div>
            </>
        ),
    },
    smartRx: {
        title: "Smart RX Insights",
        subtitle: "Look into prescription Smartly",
        videoSrc: "https://www.youtube.com/embed/dQw4w9WgXcQ",
        bannerText: "Smarter prescriptions, safer patient outcomes.",
        mainContent: (
            <>
                {/* <div className="content-head-div pt-3 pt-lg-4">
                    <img src={logo} alt="Company Logo" style={{ width: "120px", height: "52px" }} />
                    <p className="content-head-text ps-0 pt-4 pt-md-5 pt-lg-4 mb-0">&nbsp; - Your Smart Prescription Hub</p>
                </div> */}
                <div className="content-head-div pt-3 pb-2 pt-md-5 pt-lg-5">
                    <p className="content-head mb-0"><b>SmartRx</b></p>
                    
                    {/* <img src={logo} alt="Company Logo" style={{ width: "120px", height: "52px" }} /> */}
                    <p className="content-head-text mb-0 align-content-end"> – Your Smart Prescription Hub</p>
                </div>
                <div className="content-mid-text ps-3 ps-lg-3"> Register once, manage all of your medical records for lifetime.</div>
                <div className="content-description ps-3 ps-lg-3">
                    <br />
                    After uploading a prescription, you have the opportunity to request for digitization of your prescription. Once we request, you will be notified within 24 hours about the
                    completion of digitization.
                    <br />
                    <br />
                    <b>Prescription digitization?</b>
                    <br />
                    Yes, after getting confirmation, you are smart with your prescription! Get all verified, reliable information details of your prescription sections in one view. You shall observe
                    all the sections summary/overview, details, related HealthEdx at one place so that you can observe actually what happen to you or your patient. You can manage your SmartRx with the
                    below potentiality ----
                    <ul className="bullet-list">
                        <li> Upload & digitize prescriptions for you, your family and others.</li>
                        <li> Manage your every patient profiles easily where each patient has different medical record from your uploaded prescription.</li>
                        <li> View your visited doctor’s updated information, domestic & international achievements, ratings, fees & visit details. Rate your doctor too!</li>
                        <li> Track vitals, history, complaints, and advice</li>
                        <li> Get instant medicine dosages details, compare medicine prices and buy at your own decision!</li>
                        <li>
                            {" "}
                            Understand lab test list prescribed by your doctor, compare prices among different test centers and decide yourself where to go based on your reasonability, reliability and
                            comfortability.
                        </li>
                        <li> Receive regular evaluation by different charts & graphs, forecast and get suggestion from the app.</li>
                        <li> Get notified regularly for any changes happen.</li>
                        <li> Observe the details as a patient switching patient to patient, patient to logged user profile and dashboard.</li>
                        <li> Share your patient’s details, chart, graph, reports to your family member and let them understand the situation about themselves.</li>
                    </ul>
                    <div className="bottom-div d-flex">
                        <img src={logo1} alt="Company Logo" style={{ width: "70px", height: "18px" }} />
                        {/* <LogoIcon className="folderIcon" alt="Folder Icon" height={20} /> */}
                        <div className="bottom-text ps-2">All your health info—smart, simple, organized.</div>
                    </div>
                </div>
            </>
        ),
    },
    healthEdx: {
        title: "HealthEdx",
        subtitle: "HealthEdx for Health literacy",
        videoSrc: "https://www.youtube.com/embed/dQw4w9WgXcQ",
        bannerText: "Your AI-powered health education.",
        mainContent: (
            <>
                <div className="content-head-div pt-2 pb-2 pt-md-5">
                    <p className="content-head mb-0"><b>HealthEdx</b></p>
                    {/* <p className="ps-0 pe-0 mb-0" style={{ fontSize: "clamp(18px, 3vw, 24px)" }}>
                        HealthEdx
                    </p> */}
                    <p className="content-head-text ps-0 mb-0" style={{ fontSize: "clamp(13px, 2vw, 16px)", lineHeight: "clamp(24px, 3vw, 32px)" }}>
                        – Explore. Learn. Understand. Empower!{" "}
                    </p>
                </div>
                <div className="content-description ps-3 ps-lg-3">
                    <ul className="bullet-list">
                        <li> Get basic idea about your own disease, medicine, lab test prescribed by doctor</li>
                        <li> FAQ- Find answers to medicine, test & advice FAQs</li>
                        <li> Watch videos & read blog posts, contents, tutorials on diseases, medication, lab tests and related area.</li>
                        <li> Get AI-based tips & health suggestions based on your prescription, health status</li>
                        <li> Learn in-depth about your condition in simple terms</li>
                        <li> Discover everything from symptoms to recovery and survive by educating yourself!</li>
                    </ul>
                    Easy, transparent, reliable place for your health learning.
                </div>
            </>
        ),
    },
    learnAllRx: {
        title: "FileRx Archive",
        subtitle: "Organize your family Prescription",
        videoSrc: "https://www.youtube.com/embed/dQw4w9WgXcQ",
        bannerText: "RX File to take care of Your Prescription File",
        mainContent: (
            <>
                <div className="content-head-div pt-3 pt-lg-4">
                    <FolderIcon className="folderIcon" alt="Folder Icon" height={40} />
                    <p className="content-head ps-0 pe-0 pt-1 pt-md-2 pt-lg-1">FileRx</p> <p className="content-head-text ps-0 pt-2 pt-md-3 pt-lg-2 ">– Your Prescription Archive</p>
                </div>
                <div className="content-mid-text ps-3 ps-lg-3">Never lose a prescription again!</div>
                <div className="content-description ps-3 ps-lg-3">
                    You can upload unlimited prescription here in two (02) formats- image and pdf. Way to upload in the system are-
                    <div className="content-desc-icons ps-2 ps-md-5 ps-lg-5 pt-2 pb-2">
                        <div className="upload-icon pt-1 ps-0">
                            <div className="upload-circle">
                                <FiUpload />
                            </div>
                        </div>
                        <div className="pt-1 ps-0">File Upload</div>
                        <div className="responsive-line-height ps-5 ps-md-5 ps-lg-5"></div>
                        <div className="upload-icon pt-1 ps-0">
                            <div className="upload-circle">
                                <FiCamera />
                            </div>
                        </div>
                        <div className="pt-2 ps-0">Camera Capture</div>
                    </div>
                    On both option you can retake image or upload again if not seems good the file. After uploading your file, your goal is to Request for SmartRx. Also you can manipulate your file by
                    moving file folder to folder or move to main folder, preview, download, delete.
                    <ul className="bullet-list">
                        <li> Store all prescriptions securely. Once you stored your prescription here, it is the liability for us.</li>
                        <li> Create folders for each family members and separately manipulate your files for every person.</li>
                        <li> Manage your uploaded files. Access, move, view & print anytime, anywhere</li>
                        <li> Share files & profiles with others easily</li>
                    </ul>
                    <FolderIcon className="folderIcon" alt="Folder Icon" height={20} />
                    Easy, transparent, reliable place for your health records.
                </div>
                <div className="content-head-div pt-3 pt-lg-4">
                    {/* <LogoIcon className="folderIcon" alt="Folder Icon" height={40} /> */}
                    <img src={logo} alt="Company Logo" style={{ width: "120px", height: "52px" }} />
                    {/* <p className="content-head ps-0 pe-0 pt-1 pt-md-2 pt-lg-1"> </p>{" "} */}
                    <p className="content-head-text ps-0 pt-4 pt-md-5 pt-lg-4">&nbsp; - Your Smart Prescription Hub</p>
                </div>
                <div className="content-mid-text ps-3 ps-lg-3"> Register once, manage all of your medical records for lifetime.</div>
                <div className="content-description ps-3 ps-lg-3">
                    <br />
                    After uploading a prescription, you have the opportunity to request for digitization of your prescription. Once we request, you will be notified within 24 hours about the
                    completion of digitization.
                    <br />
                    <br />
                    <b>Prescription digitization?</b>
                    <br />
                    Yes, after getting confirmation, you are smart with your prescription! Get all verified, reliable information details of your prescription sections in one view. You shall observe
                    all the sections summary/overview, details, related HealthEdx at one place so that you can observe actually what happen to you or your patient. You can manage your SmartRx with the
                    below potentiality ----
                    <ul className="bullet-list">
                        <li> Upload & digitize prescriptions for you, your family and others.</li>
                        <li> Manage your every patient profiles easily where each patient has different medical record from your uploaded prescription.</li>
                        <li> View your visited doctor’s updated information, domestic & international achievements, ratings, fees & visit details. Rate your doctor too!</li>
                        <li> Track vitals, history, complaints, and advice</li>
                        <li> Get instant medicine dosages details, compare medicine prices and buy at your own decision!</li>
                        <li>
                            {" "}
                            Understand lab test list prescribed by your doctor, compare prices among different test centers and decide yourself where to go based on your reasonability, reliability and
                            comfortability.
                        </li>
                        <li> Receive regular evaluation by different charts & graphs, forecast and get suggestion from the app.</li>
                        <li> Get notified regularly for any changes happen.</li>
                        <li> Observe the details as a patient switching patient to patient, patient to logged user profile and dashboard.</li>
                        <li> Share your patient’s details, chart, graph, reports to your family member and let them understand the situation about themselves.</li>
                    </ul>
                    <div className="bottom-div d-flex">
                        <img src={logo1} alt="Company Logo" style={{ width: "70px", height: "18px" }} />
                        {/* <LogoIcon className="folderIcon" alt="Folder Icon" height={20} /> */}
                        <div className="bottom-text ps-2">All your health info—smart, simple, organized.</div>
                    </div>
                </div>
                <div className="content-head-div pt-3 pt-lg-4">
                    <p className="content-head ps-0 pe-0 pt-1 pt-md-2 pt-lg-1">HealthEdx</p> <p className="content-head-text ps-0 pt-2 pt-md-3 pt-lg-2 ">– Explore. Learn. Understand. Empower! </p>
                </div>
                <div className="content-description ps-3 ps-lg-3">
                    <ul className="bullet-list">
                        <li> Get basic idea about your own disease, medicine, lab test prescribed by doctor</li>
                        <li> FAQ- Find answers to medicine, test & advice FAQs</li>
                        <li> Watch videos & read blog posts, contents, tutorials on diseases, medication, lab tests and related area.</li>
                        <li> Get AI-based tips & health suggestions based on your prescription, health status</li>
                        <li> Learn in-depth about your condition in simple terms</li>
                        <li> Discover everything from symptoms to recovery and survive by educating yourself!</li>
                    </ul>
                    Easy, transparent, reliable place for your health learning.
                </div>
            </>
        ),
    },
};
