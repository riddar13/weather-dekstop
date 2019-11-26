using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Device.Location;


namespace WeatherApp
{
    public partial class Form1 : Form
    {
        public double lon;
        public double lat;
        public double tempC;
        public double tempF;
        public bool trig = true;

        public Form1()
        {
            InitializeComponent();
            
        }

        private void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            lat = e.Position.Location.Latitude;
            lon = e.Position.Location.Longitude;
            
        }

        private async void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            WebRequest request = WebRequest.Create("http://api.openweathermap.org/data/2.5/weather?q=Moscow&APPID=1499f87b39982a746c16f0c3ff09b18b");

            request.Method = "POST";

            request.ContentType = "application/x-www-urlencoded";

            WebResponse response = await request.GetResponseAsync();

            string answer = string.Empty;

            using (Stream s = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(s))
                {
                    answer = await reader.ReadToEndAsync();
                }
            }

            response.Close();

            richTextBox1.Text = answer;

            OpenWeather.OpenWeather oW = JsonConvert.DeserializeObject<OpenWeather.OpenWeather>(answer);

            panel1.BackgroundImage = oW.weather[0].Icon;

            label1.Text = oW.weather[0].main;

            label2.Text = oW.weather[0].description;

            tempF = oW.main.temp + 273.15;

            if (trig)
                label3.Text = "Средняя температура: " + oW.main.temp.ToString("0.##") + "°С";
            else
                label3.Text = "Средняя температура: " + tempF.ToString("0.##") + "°F";

           // tempC = oW.main.temp;
           // tempF = oW.main.temp + 273.15;

            label6.Text = "Скорость: " + oW.wind.speed.ToString() + " м/с";
            label7.Text = "Направление: " + oW.wind.deg.ToString() + "°";
            label4.Text = "Влажность: " + oW.main.humidity.ToString() + "%";
            label5.Text = "Давление: " + ((int)oW.main.pressure ).ToString() + " мм рт.ст.";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //label3.Text = "Средняя температура: " + tempC.ToString("0.##") + "°С";
            trig = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //label3.Text = "Средняя температура: " + tempF.ToString("0.##") + "°F";
            trig = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
