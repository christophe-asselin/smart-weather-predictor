using System;
using Windows.ApplicationModel.Background;
using Windows.System.Threading;
using BuildAzure.IoT.Adafruit.BME280;
using System.Diagnostics;


namespace SmartWeatherPredictor
{
    public sealed class StartupTask : IBackgroundTask
    {
        private BackgroundTaskDeferral defferal;
        private ThreadPoolTimer timer;
        private BME280Sensor bme280Sensor;
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            defferal = taskInstance.GetDeferral();

            await bme280Sensor.Initialize();

            timer = ThreadPoolTimer.CreatePeriodicTimer(PrintData, TimeSpan.FromSeconds(30));
        }

        private async void PrintData(ThreadPoolTimer timer)
        {
            var temperature = await bme280Sensor.ReadTemperature();
            var humidity = await bme280Sensor.ReadHumidity();
            var pressure = await bme280Sensor.ReadPressure();

            Debug.WriteLine(Convert.ToString(temperature));
            Debug.WriteLine(Convert.ToString(humidity));
            Debug.WriteLine(Convert.ToString(pressure));

        }
    }
}
