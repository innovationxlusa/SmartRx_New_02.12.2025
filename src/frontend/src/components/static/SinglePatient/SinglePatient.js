import MyPieChart from "../Charts/MyPieChart";


const defaultData = [
    { name: "Doctor", value: 1 },
    { name: "Medicine", value: 20 },
    { name: "Lab", value: 10 },
    { name: "Transport", value: 2000 },
    { name: "Others", value: 1000 },
    ];
    
const SinglePatient = () => {
  return (
    <>
      <div className="h1 mt-3 text-start">Cost Chart</div>
      <div className="container mb-5 pb-3">
        <div className="row">
          <div className="d-flex justify-content-center">
            <MyPieChart
              width={340}
              height={340}
              innerRadius={90}
              outerRadius={155}
              cy={170}
              cx={170}
              circle={false}
              font={true}
              data={defaultData}
            />
          </div>
        </div>
      </div>
    </>
  );
};

export default SinglePatient;
