import { useEffect, useState } from "react";
import { Worker, Viewer } from "@react-pdf-viewer/core";
import { defaultLayoutPlugin } from "@react-pdf-viewer/default-layout";

import "@react-pdf-viewer/core/lib/styles/index.css";
import "@react-pdf-viewer/default-layout/lib/styles/index.css";

const PDFViewer = ({ file }) => {
  const [pdfUrl, setPdfUrl] = useState(null);
  const defaultLayoutPluginInstance = defaultLayoutPlugin();

  useEffect(() => {
    if (file) {
      const safeBlob = new Blob([file], { type: "application/pdf" });
      const fileURL = URL.createObjectURL(safeBlob);
      setPdfUrl(fileURL);

      return () => URL.revokeObjectURL(fileURL);
    }
  }, [file]);

  return (
    <div style={{ height: "350px", marginTop: "20px" }}>
      <Worker>
        {/* <Viewer fileUrl={pdfUrl} plugins={[defaultLayoutPluginInstance]} renderError={(error) => <div style={{ color: "red" }}>⚠️ PDF failed to load</div>} /> */}
        {pdfUrl ? (
          <Viewer
            fileUrl={pdfUrl}
            plugins={[defaultLayoutPluginInstance]}
            renderError={(error) => (
              <div style={{ color: "red", marginTop: "0px" }}>
                ⚠️ PDF failed to load. Try downloading it manually. {error}
              </div>
            )}
          />
        ) : (
          <div style={{ padding: "1rem" }}>No PDF URL provided</div>
        )}
      </Worker>
    </div>
  );
};

export default PDFViewer;
