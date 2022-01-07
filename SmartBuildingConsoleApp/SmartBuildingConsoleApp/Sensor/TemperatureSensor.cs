using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBuildingConsoleApp.Sensor
{
    public class TemperatureSensor
    {
        private double temperature = 22;
        private int minimumTemperature = 22;
        private int maximumTemperature = 50;

        Random rand = new Random();

        public TemperatureSensor()
        {
            temperature = rand.Next(minimumTemperature, maximumTemperature);
        }

        public double GetMeasurement()
        {
            double step = rand.Next(-6, 6);
            double temperatureStep = step * rand.NextDouble();

            temperature += temperatureStep;

            if (temperature < (double)minimumTemperature)
            {
                temperature = (double)minimumTemperature;
            }

            if (temperature  > (double)maximumTemperature)
            {
                temperature = (double)maximumTemperature;
            }

            return temperature;
        }

    }
}
