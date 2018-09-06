using System;
using Windows.ApplicationModel.Background;
using BuildAzure.IoT.Adafruit.BME280;
// using System.Diagnostics;
using Microsoft.Azure.Devices.Client;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;

namespace SmartWeatherPredictor
{
    public sealed class StartupTask : IBackgroundTask
    {
        // private BackgroundTaskDeferral defferal;

        private string deviceKey = "9+2uycUmN/b+mgtLCMGvYvMGtqKSDagcyw1+ytOX76k=";
        private string _deviceId = "MyDevice";
        private string iotHubHostName = "christophe-asselin-iot-hub.azure-devices.net";
        private DeviceAuthenticationWithRegistrySymmetricKey deviceAuthentication;
        private DeviceClient deviceClient;

        private BME280Sensor bme280Sensor;
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            // defferal = taskInstance.GetDeferral();

            deviceAuthentication = new DeviceAuthenticationWithRegistrySymmetricKey(_deviceId, deviceKey);
            deviceClient = DeviceClient.Create(iotHubHostName, deviceAuthentication, TransportType.Mqtt);

            await bme280Sensor.Initialize();

            int _messageId = 0;

            while (true)
            {
                var currentTemperature = await bme280Sensor.ReadTemperature();
                var currentHumidity = await bme280Sensor.ReadHumidity();
                var currentPressure = await bme280Sensor.ReadPressure();

                var telemetryDataPoint = new
                {
                    messageId = _messageId++,
                    deviceId = _deviceId,
                    tempereture = currentTemperature,
                    humidity = currentHumidity,
                    pressure = currentPressure
                };
                string messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                Message message = new Message(Encoding.ASCII.GetBytes(messageString));

                await deviceClient.SendEventAsync(message);
                Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);

                await Task.Delay(15000);
            }
        }

    }
}
