import HalfRoundedPieChart from "../Charts/HalfRoundedPieChart";

const BmiChart = () => {
    return (
        <>
            <div className="h1 mt-3 text-start">
                BMI Chart
            </div>
            <div className="container mb-5 pb-3">
                <div className="row">
                    <div className="d-flex justify-content-center">
                        <HalfRoundedPieChart 
                            width={400} 
                            height={400} 
                            innerRadius={120} 
                            outerRadius={190} 
                            cy={200} 
                            cx={200}
                            circle={false}
                            font={true}
                        />
                    </div>
                </div>
            </div>
        </>
    );
}

export default BmiChart;
