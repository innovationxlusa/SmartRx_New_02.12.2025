using PMSBackend.Application.DTOs;
using PMSBackend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSBackend.Application.CommonServices.PatientSmartRx
{
    public class CalcualteVitalMeasurements
    {

        public static SmartRxVital CalcualteVitalDataMeasurements(SmartRx_PatientVitalsEntity vt, ref string diastolic, ref decimal diastolicValue, ref decimal? diastolicLowValue, ref decimal? diastolicMediumValue, ref decimal? diastolicHighValue, ref string systolic, ref decimal systolicValue, ref decimal? systolicLowValue, ref decimal? systolicMediumValue, ref decimal? systolicHighValue, ref string systolicStatus, ref string diastolicStatus)
        {
            var vital = new SmartRxVital();
            var vtVitalDetails = vt.Vital;
            var measurement = vt.Vital.Unit;
            vital.Id = vt.Id;
            vital.PrescriptionId = vt.PrescriptionId;
            vital.SmartRxMasterId = vt.SmartRxMasterId;
            vital.VitalId = vt.VitalId;
            vital.VitalValue = vt.VitalValue;
            vital.VitalValueString = Common.FormatDecimal(Convert.ToDecimal(vt.VitalValue));
            vital.Code = vtVitalDetails.Code;
            vital.Name = vtVitalDetails.Name;
            vital.Description = vt.Vital.Description;
            vital.ApplicableEntity = vtVitalDetails.ApplicableEntity!;

            if (vtVitalDetails.Name == "Blood Pressure" && vtVitalDetails.ApplicableEntity is not null)
            {
                if (vtVitalDetails.ApplicableEntity == "Diastolic")
                {
                    diastolic = vtVitalDetails.Name;
                    diastolicValue = vt.VitalValue;
                    diastolicLowValue = vtVitalDetails.LowRange;
                    diastolicMediumValue = vtVitalDetails.MidNextRange;
                    diastolicHighValue = vtVitalDetails.HighRange;
                }
                if (vtVitalDetails.ApplicableEntity == "Systolic")
                {
                    systolic = vtVitalDetails.Name;
                    systolicValue = vt.VitalValue;
                    systolicLowValue = vtVitalDetails.LowRange;
                    systolicMediumValue = vtVitalDetails.MidNextRange;
                    systolicHighValue = vtVitalDetails.HighRange;
                }
                if (systolic.Length > 0 && diastolic.Length > 0)
                {
                    if ((diastolicValue < diastolicLowValue) || (diastolicValue < diastolicLowValue && systolicValue < systolicLowValue) || systolicValue < systolicLowValue)
                        systolicStatus = diastolicStatus = "Low";
                    else if (diastolicValue < diastolicMediumValue &&
                             systolicValue >= systolicLowValue && systolicValue <= systolicMediumValue)
                        systolicStatus = diastolicStatus = "Normal";
                    else if (systolicValue > systolicHighValue)
                        systolicStatus = diastolicStatus = "High";

                    // Fallback: If one is null, use the other
                    vital.Status = systolicStatus ?? diastolicStatus ?? "Unknown";

                    vital.VitalValueString = $"{Common.FormatDecimal(Convert.ToDecimal(systolicValue))}/{Common.FormatDecimal(Convert.ToDecimal(diastolicValue))}";
                }

                vital.VitalValueStandardString = $"120/80";
            }
            if (vtVitalDetails.Name == "Body Temperature" && vtVitalDetails.ApplicableEntity is not null && vtVitalDetails.ApplicableEntity == "Farenheit")
            {
                if (vt.VitalValue < vtVitalDetails.LowRange) vital.Status = "Low";
                else if (vt.VitalValue >= vtVitalDetails.LowRange && vt.VitalValue <= vtVitalDetails.MidNextRange) vital.Status = "Normal";
                else if (vt.VitalValue >= vtVitalDetails.HighRange) vital.Status = "High";
                vital.VitalValueStandardString = $"{Common.FormatDecimal(Convert.ToDecimal(vtVitalDetails.MidRange))}-{Common.FormatDecimal(Convert.ToDecimal(vtVitalDetails.MidNextRange))}";
            }
            if (vtVitalDetails.Name == "Pulse Rate")
            {
                if (vt.VitalValue < vtVitalDetails.MidRange) vital.Status = vtVitalDetails.LowStatus;
                else if (vt.VitalValue >= vtVitalDetails.MidRange && vt.VitalValue < vtVitalDetails.HighRange) vital.Status = vtVitalDetails.MidNextStatus;
                else if (vt.VitalValue >= vtVitalDetails.HighRange) vital.Status = vtVitalDetails.HighStatus;
                vital.VitalValueStandardString = $"{Common.FormatDecimal(Convert.ToDecimal(vtVitalDetails.MidRange))}-{Common.FormatDecimal(Convert.ToDecimal(vtVitalDetails.HighRange))}";
            }
            if (vtVitalDetails.Name == "Respiratory Rate")
            {
                if (vt.VitalValue < vtVitalDetails.LowRange) vital.Status = vtVitalDetails.LowStatus;
                else if (vt.VitalValue >= vtVitalDetails.LowRange && vt.VitalValue <= vtVitalDetails.HighRange) vital.Status = vtVitalDetails.MidNextStatus;
                else if (vt.VitalValue > vtVitalDetails.HighRange) vital.Status = vtVitalDetails.HighStatus;
                vital.VitalValueStandardString = $"{Common.FormatDecimal(Convert.ToDecimal(vtVitalDetails.LowRange))}-{Common.FormatDecimal(Convert.ToDecimal(vtVitalDetails.MidNextRange))}";
            }
            if (vtVitalDetails.Name == "Blood Oxygen")
            {
                if (vt.VitalValue < vtVitalDetails.LowRange) vital.Status = vtVitalDetails.LowStatus;
                else if (vt.VitalValue >= vtVitalDetails.LowRange && vt.VitalValue <= vtVitalDetails.HighRange) vital.Status = vtVitalDetails.MidNextStatus;
                else if (vt.VitalValue > vtVitalDetails.HighRange) vital.Status = vtVitalDetails.HighStatus;
                vital.VitalValueStandardString = $"{Common.FormatDecimal(Convert.ToDecimal(vtVitalDetails.LowRange))}-{Common.FormatDecimal(Convert.ToDecimal(vtVitalDetails.MidRange))}";
            }

            if (vtVitalDetails.Name == "Blood Glucose")
            {
                if (vt.VitalValue < vtVitalDetails.LowRange) vital.Status = vtVitalDetails.LowStatus;
                else if (vt.VitalValue >= vtVitalDetails.LowRange && vt.VitalValue <= vtVitalDetails.HighRange) vital.Status = vtVitalDetails.MidNextStatus;
                else if (vt.VitalValue > vtVitalDetails.HighRange) vital.Status = vtVitalDetails.HighStatus;
                vital.VitalValueStandardString = $"{Common.FormatDecimal(Convert.ToDecimal(vtVitalDetails.LowRange))} - {Common.FormatDecimal(Convert.ToDecimal(vtVitalDetails.MidRange))}";
            }

            vital.MeasurementUnit = measurement.MeasurementUnit;
            vital.MeasurementUnitDetails = measurement.Details;
            return vital;
        }

    }
}
