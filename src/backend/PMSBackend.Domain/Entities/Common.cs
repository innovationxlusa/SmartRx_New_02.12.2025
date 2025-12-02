using System.ComponentModel;
using System.Reflection;

namespace PMSBackend.Domain.Entities
{
    public static class Common
    {
        public static string GetStringWithoutSpaceWithLowerCase(string str)
        {
            var customStr = string.Join("", str.Trim().ToLower().Split(' '));
            return customStr;
        }
        public static string GetEnumDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute != null ? attribute.Description : value.ToString();
        }
        public static bool DescriptionContainsText<TEnum>(string searchText) where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Any(value =>
                {
                    var field = typeof(TEnum).GetField(value.ToString());
                    var descriptionAttribute = field?.GetCustomAttribute<DescriptionAttribute>();
                    return descriptionAttribute != null &&
                           descriptionAttribute.Description.Contains(searchText, StringComparison.OrdinalIgnoreCase);
                });
        }

        public static string FormatDecimal(decimal value) =>
                value % 1 == 0 ? ((int)value).ToString() : value.ToString("0.##");


        public static async Task<List<Configuration_DistrictEntity>> DistrictList()
        {
            List<Configuration_DistrictEntity> list = new List<Configuration_DistrictEntity>();

            //Rangpur Division
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.RangpurDivision, Code = "43", Name = "Dinajpur", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.RangpurDivision, Code = "44", Name = "Gaibandha", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.RangpurDivision, Code = "45", Name = "Kurigram", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.RangpurDivision, Code = "46", Name = "Lalmonirhat", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.RangpurDivision, Code = "47", Name = "Nilphamari", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.RangpurDivision, Code = "48", Name = "Panchagarh", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.RangpurDivision, Code = "49", Name = "Rangpur", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.RangpurDivision, Code = "50", Name = "Thakurgaon", CreatedById = 2, CreatedDate = DateTime.Now });


            //Rajshahi Division
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.RajshahiDivision, Code = "35", Name = "Bogra", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.RajshahiDivision, Code = "36", Name = "Joypurhat", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.RajshahiDivision, Code = "37", Name = "Naogaon", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.RajshahiDivision, Code = "38", Name = "Natore", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.RajshahiDivision, Code = "39", Name = "Chapainawabganj", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.RajshahiDivision, Code = "40", Name = "Pabna", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.RajshahiDivision, Code = "41", Name = "Rajshahi", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.RajshahiDivision, Code = "42", Name = "Sirajganj", CreatedById = 2, CreatedDate = DateTime.Now });

            //Khulna Division
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.KhulnaDivision, Code = "25", Name = "Bagerhat", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.KhulnaDivision, Code = "26", Name = "Chuadanga", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.KhulnaDivision, Code = "27", Name = "Jashore", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.KhulnaDivision, Code = "28", Name = "Jhenaidah", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.KhulnaDivision, Code = "29", Name = "Khulna", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.KhulnaDivision, Code = "30", Name = "Kushtia", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.KhulnaDivision, Code = "31", Name = "Magura", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.KhulnaDivision, Code = "32", Name = "Meherpur", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.KhulnaDivision, Code = "33", Name = "Narail", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.KhulnaDivision, Code = "34", Name = "Satkhira", CreatedById = 2, CreatedDate = DateTime.Now });

            //Barishal Division
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.BarishalDivision, Code = "55", Name = "Barguna", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.BarishalDivision, Code = "56", Name = "Barishal", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.BarishalDivision, Code = "57", Name = "Bhola", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.BarishalDivision, Code = "58", Name = "Jhalokati", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.BarishalDivision, Code = "59", Name = "Patuakhali", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.BarishalDivision, Code = "60", Name = "Pirojpur", CreatedById = 2, CreatedDate = DateTime.Now });

            //Mymensingh Division
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.MymensinghDivision, Code = "61", Name = "Jamalpur", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.MymensinghDivision, Code = "62", Name = "Mymensingh", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.MymensinghDivision, Code = "63", Name = "Netrokona", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.MymensinghDivision, Code = "64", Name = "Sherpur", CreatedById = 2, CreatedDate = DateTime.Now });

            // Dhaka Division
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.DhakaDivision, Code = "01", Name = "Dhaka", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.DhakaDivision, Code = "02", Name = "Faridpur", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.DhakaDivision, Code = "03", Name = "Gazipur", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.DhakaDivision, Code = "04", Name = "Gopalganj", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.DhakaDivision, Code = "05", Name = "Kishoreganj", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.DhakaDivision, Code = "06", Name = "Madaripur", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.DhakaDivision, Code = "07", Name = "Manikganj", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.DhakaDivision, Code = "08", Name = "Munshiganj", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.DhakaDivision, Code = "09", Name = "Narayanganj", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.DhakaDivision, Code = "10", Name = "Narsingdi", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.DhakaDivision, Code = "11", Name = "Rajbari", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.DhakaDivision, Code = "12", Name = "Shariatpur", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.DhakaDivision, Code = "13", Name = "Tangail", CreatedById = 2, CreatedDate = DateTime.Now });

            //Sylhet Division
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.SylhetDivision, Code = "51", Name = "Habiganj", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.SylhetDivision, Code = "52", Name = "Moulvibazar", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.SylhetDivision, Code = "53", Name = "Sunamganj", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.SylhetDivision, Code = "54", Name = "Sylhet", CreatedById = 2, CreatedDate = DateTime.Now });


            // Chattogram (Chittagong) Division
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.ChattogramDivision, Code = "14", Name = "Bandarban", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.ChattogramDivision, Code = "15", Name = "Brahmanbaria", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.ChattogramDivision, Code = "16", Name = "Chandpur", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.ChattogramDivision, Code = "17", Name = "Chattogram", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.ChattogramDivision, Code = "18", Name = "Cumilla", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.ChattogramDivision, Code = "19", Name = "Cox's Bazar", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.ChattogramDivision, Code = "20", Name = "Feni", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.ChattogramDivision, Code = "21", Name = "Khagrachhari", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.ChattogramDivision, Code = "22", Name = "Lakshmipur", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.ChattogramDivision, Code = "23", Name = "Noakhali", CreatedById = 2, CreatedDate = DateTime.Now });
            list.Add(new Configuration_DistrictEntity() { DivisionId = (int)Divisions.ChattogramDivision, Code = "24", Name = "Rangamati", CreatedById = 2, CreatedDate = DateTime.Now });

            await Task.CompletedTask;
            return list;
        }

        public static async Task<List<Configuration_PoliceStationEntity>> ThanaList()
        {
            List<Configuration_PoliceStationEntity> list = new List<Configuration_PoliceStationEntity>();

            //Rangpur Division
            // list.Add(new ConfigurationThanaEntity() { DistrictId= Code=0, Name = "Dinajpur", CreatedById = 2, CreatedDate = DateTime.Now });

            await Task.CompletedTask;
            return list;
        }



    }

    public static class BmiCalculator
    {
        public static decimal? CalculateBmi(int feet, decimal inches, decimal weightKg)
        {
            if (feet < 0 || inches < 0 || inches >= 12 || weightKg <= 0)
                return null;

            // Convert total height to meters
            double totalInches =Convert.ToDouble((feet * 12) + inches);
            double heightMeters = (double)(totalInches * 0.0254);
            double weight=Convert.ToDouble(weightKg);
            // Calculate BMI
            double bmi = (double)(weight / (heightMeters * heightMeters));
            return (decimal?)Math.Round(bmi, 1);
        }
        public static Tuple<int, int> ConvertDecimalHeightToFeetInches(double height)
        {
            int feet = (int)Math.Floor(height);

            string[] split = height.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture).Split('.');
            int inches = 0;

            if (split.Length == 2)
            {
                // Take up to 2 digits after decimal (e.g., "11" from "5.11")
                string inchPart = split[1].PadRight(2, '0').Substring(0, 2);
                if (int.TryParse(inchPart, out int parsedInches))
                {
                    // Clamp to 0–11 inches
                    inches = Math.Min(Math.Max(parsedInches, 0), 11);
                }
            }

            return new Tuple<int, int>(feet, inches);
        }
        public static Tuple<int, int> SeparateFeetAndInches(decimal height)
        {
            int feet = (int)Math.Floor(height);

            string[] split = height.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture).Split('.');
            int inches = 0;

            if (split.Length == 2)
            {
                // Take up to 2 digits after decimal (e.g., "11" from "5.11")
                string inchPart = split[1].PadRight(2, '0').Substring(0, 2);
                if (int.TryParse(inchPart, out int parsedInches))
                {
                    // Clamp to 0–11 inches
                    inches = Math.Min(Math.Max(parsedInches, 0), 11);
                }
            }

            return new Tuple<int, int>(feet, inches);
        }
    }

    public enum Status
    {
        Active = 1,
        InActive = 2
    }
    public enum LoginType
    {
        UserName = 1,
        Mobile = 2,
        Google = 3,
        Facebook = 4,
        Twitter = 5,
        Email = 6
    }
    public enum Gender
    {
        Male = 1,
        Female = 2
    }
    public enum Roles
    {
        [Description("Super Admin")]
        superadmin = 1,
        [Description("Admin")]
        admin = 2,
        [Description("Entry User")]
        entryuser = 3,
        [Description("Approver")]
        approver = 4,
        [Description("Recommender")]
        recommender = 5,
        [Description("External User")]
        externaluser = 6
    }
    public enum MaritalStatuses
    {
        Unmarried = 1,
        Married = 2,
        Divorced = 3,
        Widowed = 4,
        Separated = 5
    }
    public enum BloodGroups
    {
        A_Positive = 1,    // A+
        A_Negative = 2,    // A-
        B_Positive = 3,    // B+
        B_Negative = 4,    // B-
        AB_Positive = 5,   // AB+
        AB_Negative = 6,   // AB-
        O_Positive = 7,    // O+
        O_Negative = 8     // O-
    }
    public enum Vitals
    {
        [Description("Blood Pressure")]
        BloodPressure = 1,
        [Description("Body Temperature")]
        BodyTemperature = 2,
        [Description("Pulse Rate")]
        PulseRate = 3,
        [Description("Respiratory Rate")]
        RespirationRate = 4,
        [Description("Blood Oxygen")]
        BloodOxygen = 5,
        [Description("Height")]
        Height = 6,
        [Description("Weight")]
        Weight = 7,
        [Description("Blood Glucose")]
        BloodGlucoseLevel = 8,
        [Description("BMI")]
        BodyMassIndex = 9
    }
    public enum Divisions
    {
        [Description("Rangpur Division")]
        RangpurDivision = 1,
        [Description("Rajshahi Division")]
        RajshahiDivision = 2,
        [Description("Khulna Division")]
        KhulnaDivision = 3,
        [Description("Barishal Division")]
        BarishalDivision = 4,
        [Description("Mymensingh Division")]
        MymensinghDivision = 5,
        [Description("Dhaka Division")]
        DhakaDivision = 6,
        [Description("Sylhet Division")]
        SylhetDivision = 7,
        [Description("Chattogram Division")]
        ChattogramDivision = 8
    }
    public enum DistrictCode
    {
        // Rangpur Division
        Panchagar = 1,
        Thakurgaon = 2,
        Dinajpur = 3,
        Nilphamari = 4,
        Lalmonirhat = 5,
        Rangpur = 6,
        Kurigram = 7,
        Gaibandha = 8,

        // Rajshahi Division
        Joypurhat = 9,
        Bogura = 10,
        Naogaon = 11,
        Natore = 12,
        ChapaiNawabganj = 13,
        Rajshahi = 14,
        Sirajganj = 15,
        Pabna = 16,

        // Khulna Division
        Kushtia = 17,
        Meherpur = 18,
        Chuadanga = 19,
        Jhenaidah = 20,
        Magura = 21,
        Narail = 22,
        Jashore = 23,
        Satkhira = 24,
        Khulna = 25,
        Bagerhat = 26,

        // Barishal Division
        Pirojpur = 27,
        Jhalokati = 28,
        Barishal = 29,
        Bhola = 30,
        Patuakhali = 31,
        Barguna = 32,

        // Mymensingh Division       
        Netrokona = 33,
        Mymensingh = 34,
        Sherpur = 35,
        Jamalpur = 36,

        // Dhaka Division
        Tangail = 37,
        Kishoreganj = 38,
        Manikganj = 39,
        Dhaka = 40,
        Gazipur = 41,
        Narsingdi = 42,
        Narayanganj = 43,
        Munshiganj = 44,
        Faridpur = 45,
        Rajbari = 46,
        Gopalganj = 47,
        Madaripur = 48,
        Shariatpur = 49,

        // Sylhet Division
        Sunamganj = 50,
        Sylhet = 51,
        Moulvibazar = 52,
        Habiganj = 53,

        // Chattogram Division       
        Brahmanbaria = 54,
        Cumilla = 55,
        Chandpur = 56,
        Lakshmipur = 57,
        Noakhali = 58,
        Feni = 59,
        Chattogram = 60,
        CoxsBazar = 61,
        Khagrachhari = 62,
        Rangamati = 63,
        Bandarban = 64
    }

}

//public enum Districts
//    {
//        [Description("Rangpur Division")]
//        Panchagar = "01",//01= Panchagar
//    }


public enum RewardType
{
    None = 0,
    Noncashable=1,
    Cashable=2,
    Money=3
}

public enum BadgeType
{
    PointsMilestone = 1, // based on total points earned
    ActivityBased = 2,   // based on activity count
    Special = 3          // admin or event based
}