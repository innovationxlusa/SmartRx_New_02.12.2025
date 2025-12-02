namespace PMSBackend.Domain.SharedContract
{
    public class DoctorChamberContract
    {
        public long DoctorId { get; set; }
        public long? HospitalId { get; set; }
        public string? HospitalName { get; set; }
        public long? DepartmentSectionId { get; set; }
        public string? DepartmentSectionName { get; set; }
        public long? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public bool IsMainChamber { get; set; }
        public long? ChamberId { get; set; }
        public string? ChamberName { get; set; }
        public string? ChamberAddress { get; set; }
        public long? ChamberCityId { get; set; }
        public string? ChamberCityName { get; set; }
        public string? ChamberPostalCode { get; set; }
        public string? ChamberDescription { get; set; }
        public string? ChamberGoogleAddress { get; set; }
        public string? ChamberGoogleRating { get; set; }
        public string? ChamberDoctorBookingMobileNos { get; set; }
        public string? ChamberHelpline { get; set; }
        public string? ChamberEmail { get; set; }
        public string? ChamberVisitingHour { get; set; }
        public string? ChamberOpenDay { get; set; }
        public string? ChamberCloseDay { get; set; }
        public string? ChamberStartTime { get; set; }
        public string? ChamberEndTime { get; set; }
        public string? ChamberOtherDoctorsId { get; set; }

        public long? DoctorDesignationInChamberId { get; set; }
        public string? DoctorDesignaitonInChamber {  get; set; }
        public string? VisitingHour { get; set; }
        public string? Remarks { get; set; }
        public string? DoctorSpecialization { get; set; }
        public string? DoctorVisitingDaysInChamber { get; set; }
        public bool IsActive { get; set; }
        public string? HospitalLogoFileName { get; set; }
        public string? HospitalLogoFilePath { get; set; }
        public string? HospitalLogoFileExtension { get; set; }
    }
}
