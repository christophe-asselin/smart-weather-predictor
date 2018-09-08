namespace SmartWeatherPredictor
{
    /// <summary>
    /// Information for connecting to you IoT-Hub.
    /// </summary>
    public sealed partial class StartupTask
    {
        static string _iotHubUri = "<Your IoT-Hub connection string>";
        static string _deviceKey = "<Your device connection key>";
        static string _deviceId = "<Your deviceId>";
    }
}
