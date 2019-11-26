using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.OpenWeather
{
    class main
    {
        private double _temp;

        public double temp
        {
            get
            {
                return _temp;
            }
            set
            {
                _temp = value - 273.15; //перевод в цельсии
            }
        }

        private double _pressure;

        public double pressure
        {
            get
            {
                return _pressure;
            }
            set
            {
                _pressure = value / 1.3332239; //перевод в мл ртутного столба
            }
        }
    }
}
