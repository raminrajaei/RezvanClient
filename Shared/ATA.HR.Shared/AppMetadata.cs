using System.Reflection;

namespace ATA.HR.Shared
{
    public static class AppMetadata
    {
        public static readonly string AppVersion = Assembly.GetExecutingAssembly().GetName().Version!.ToString(3);

        public static readonly string SSOAppName = "employmentcontract";

        public static readonly string PersianFullName = "سامانه‌ی جامع منابع انسانی";

        public static readonly string EnglishFullName = "ATA Employment Contract";

        public static readonly string SolutionName = "ATA.HR";
    }
}