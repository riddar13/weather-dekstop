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
        public String city;
        public String an;
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

        public async void handler(String queue)
        {
            WebRequest request = WebRequest.Create(queue);
            request.Method = "POST";
            request.ContentType = "application/x-www-urlencoded";
            try
            {
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

                an = answer;

                OpenWeather.OpenWeather oW = JsonConvert.DeserializeObject<OpenWeather.OpenWeather>(answer);
                city = oW.name;
                label8.Text = this.city;
                tempF = (oW.main.temp - 32)*5/9;
                tempC = oW.main.temp;

                panel1.BackgroundImage = oW.weather[0].Icon;
                label1.Text = oW.weather[0].main;
                label2.Text = oW.weather[0].description;

                if (trig)
                    label3.Text = "Средняя температура: " + oW.main.temp.ToString("0.##") + "°С";
                else
                    label3.Text = "Средняя температура: " + tempF.ToString("0.##") + "°F";

                label6.Text = "Скорость: " + oW.wind.speed.ToString() + " м/с";
                label7.Text = "Направление: " + oW.wind.deg.ToString() + "°";
                label4.Text = "Влажность: " + oW.main.humidity.ToString() + "%";
                label5.Text = "Давление: " + ((int)oW.main.pressure).ToString() + " мм рт.ст.";
            }
            catch (System.Net.WebException e)
            {
                MessageBox.Show(e.Message + "Город не найден");
            }
        }

        private async void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.Text = an;    
        }

        private void button1_Click(object sender, EventArgs e)
        {
            trig = true;
            label3.Text = "Средняя температура: " + tempC.ToString("0.##") + "°С";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            trig = false;
            label3.Text = "Средняя температура: " + tempF.ToString("0.##") + "°F";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            String newCity;
            newCity = this.city;
            textBox1.Clear();
            String queue = "https://api.openweathermap.org/data/2.5/weather?q=" + newCity + "&APPID=1499f87b39982a746c16f0c3ff09b18b&lang=ru";
            handler(queue);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            String queue = "https://api.openweathermap.org/data/2.5/weather?lat=" + lat + "&lon=" + lon + "&APPID=1499f87b39982a746c16f0c3ff09b18b";
            handler(queue);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            city = textBox1.Text;
        }
    }
}
