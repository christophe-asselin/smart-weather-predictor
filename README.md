# smart-weather-predictor
A smart IoT weather predictor which uses Azure services such as machine learning (WORK IN PROGRESS).

The weather prediction feature is not yet implemented. The data from the IoT device is sent to an Azure IoT-Hub and can then be used
as wished.
I deployed a sample web app from Microsoft (https://docs.microsoft.com/en-us/azure/iot-hub/iot-hub-live-data-visualization-in-web-apps)
to visualize the temperature and humidity measured by my IoT device. Here is the link for the web app that is connected to my IoT-Hub:
https://weather-web-app.azurewebsites.net/

The website might or might not show data depending if my setup is up and running or not.

This project uses a Raspberry Pi and a BME280 temperature/humidity/pressure/altitude sensor. The following Frtizing schematics shows how
to wire up the sensor:

![Alt text](BME280Fritzing.png?raw=true "Fritzing diagram")
