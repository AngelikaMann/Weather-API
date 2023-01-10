using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Windows;
using System.Windows.Media.Imaging;

namespace AppWeather2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string apiKey = "a194f987ff708686271ee4499c7e023c";

        private string requestUrl = "https://api.openweathermap.org/data/2.5/weather";

        public MainWindow()
        {
            InitializeComponent();
            UpdateData("Dortmund");

        }

        public void UpdateData(string city)
        {
            WeatherMapResponse result = GetWeatherData(city);

            string finalImage = "snow.png";
            string currentWeather = result.weather[0].main.ToLower();
            if (currentWeather.Contains("cloud"))
            {
                finalImage = "cloud.png";
            }
            else if (currentWeather.Contains("rain"))
            {
                finalImage = "rain.png";
            }
            else if (currentWeather.Contains("sun"))
            {
                finalImage = "sun.png";
            }

            backgroundImage.ImageSource = new BitmapImage(new Uri("Images/" + finalImage, UriKind.Relative));
            labelTemperature.Content = result.main.temp.ToString("F1") + "°C";
            labelInfo.Content = result.weather[0].main;
        }

        public WeatherMapResponse GetWeatherData(string city)
        {
            HttpClient httpClient = new HttpClient();
            var finalUrl = requestUrl + "?q=" + city + "&appid=" + apiKey + "&units=metric";
            HttpResponseMessage httpResponse = httpClient.GetAsync(finalUrl).Result;
            string response = httpResponse.Content.ReadAsStringAsync().Result;
            //Console.WriteLine(response);
            WeatherMapResponse weatherMapResponse = JsonConvert.DeserializeObject<WeatherMapResponse>(response);
            return weatherMapResponse;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string query = textBoxQuery.Text;
            UpdateData(query);
        }
    }
}
