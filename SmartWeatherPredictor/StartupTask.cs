using System;
using Windows.ApplicationModel.Background;
using Microsoft.Azure.Devices.Client;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using System.Diagnostics;

namespace SmartWeatherPredictor
{
    public sealed partial class StartupTask : IBackgroundTask
    {
        private BackgroundTaskDeferral defferal;

        private DeviceAuthenticationWithRegistrySymmetricKey deviceAuthentication;
        private DeviceClient deviceClient;

        private BuildAzure.IoT.Adafruit.BME280.BME280Sensor bme280Sensor;
        private float currentTemperature;
        private float currentHumidity;
        private float currentPressure;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            defferal = taskInstance.GetDeferral();

            deviceAuthentication = new DeviceAuthenticationWithRegistrySymmetricKey(_deviceId, _deviceKey);
            deviceClient = DeviceClient.Create(_iotHubUri, deviceAuthentication, TransportType.Mqtt);

            bme280Sensor = new BuildAzure.IoT.Adafruit.BME280.BME280Sensor();
            await bme280Sensor.Initialize();

            int _messageId = 0;

            while (true)
            {
                SendTelemetry(_messageId);
   
                await Task.Delay(15000);
            }
        }

        private async void SendTelemetry(int msgId)
        {
            try
            {
                currentTemperature = await bme280Sensor.ReadTemperature();
                currentHumidity = await bme280Sensor.ReadHumidity();
                currentPressure = (await bme280Sensor.ReadPressure()) / 1000;


                var telemetryDataPoint = new
                {
                    messageId = msgId++,
                    deviceId = _deviceId,
                    temperature = currentTemperature,
                    humidity = currentHumidity,
                    pressure = currentPressure
                };

                string messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                Message message = new Message(Encoding.ASCII.GetBytes(messageString));

                Debug.WriteLine(messageString);

                //await deviceClient.SendEventAsync(message);
                //Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

    }
}
