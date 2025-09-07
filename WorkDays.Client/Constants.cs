using Microsoft.Maui.Devices;

namespace WorkDays.Client.Constants
{
    public static class ApiEndpoints
    {
        public const string WorkDay = "api/workday";
        public static readonly string LocalUriScheme = DeviceInfo.Platform == DevicePlatform.Android ? "10.0.2.2" : "localhost";
        public static readonly string Scheme = DeviceInfo.Platform == DevicePlatform.Android ? "https" : "http";
        public static readonly string Port = Scheme == "https" ? "7194" : "5287";
        public static readonly string BaseUrl = $"{Scheme}://{LocalUriScheme}:{Port}";
    }
}
