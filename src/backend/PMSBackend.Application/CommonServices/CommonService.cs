namespace PMSBackend.Application.CommonServices
{
    public static class CommonService
    {
        public static bool HasImageExtension(this string source)
        {
            return (source.EndsWith(".png") || source.EndsWith(".jpg"));
        }
        public static string GenerateOtp(string mobileNumber)
        {
            var otp = new Random().Next(100000, 999999).ToString();
            return otp;
        }

    }
}
