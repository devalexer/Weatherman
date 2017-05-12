using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Weatherman
{
    class Program
    {
        static string RequestUserName()
        {
            Console.WriteLine("Please enter your name: ");
            string userName = Console.ReadLine().ToUpper();

            return userName;
        }

        static string RequestUserZip()
        {
            Console.WriteLine("Please enter your zip code: ");
            string userZip = Console.ReadLine();

            return userZip;
        }

        static RootObject GetWeather()
        {
            var url = $"http://api.openweathermap.org/data/2.5/weather?zip={RequestUserZip()},us&units=imperial&appid=bcf7aaa93424ebce8079e123df0cf3b9";
            var request = WebRequest.Create(url);
            var response = request.GetResponse();
            var rawResponse = String.Empty;

            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                rawResponse = reader.ReadToEnd();
                Console.WriteLine(rawResponse);
            }

            var forecast = JsonConvert.DeserializeObject<RootObject>(rawResponse);

            Console.WriteLine();
            Console.WriteLine($"{forecast.main.temp}*F degrees");
            Console.WriteLine($"With {forecast.weather.First().description} skies");
            return forecast;
        }

        //Add info to SQL tables: save the temperature, current conditions, the user's name and that it happened in a database
        static public void AddForecast(RootObject WeatherData, string userName)
        {
            const string connectionString =
                @"Server=localhost\SQLEXPRESS;Database=Weatherman;Trusted_Connection=True;";

            using (var connection = new SqlConnection(connectionString))
            {
                var text = @"INSERT INTO Forecast (Temperature, CurrentConditions, UsersName)" +
                           "Values (@Temperature, @CurrentConditions, @UsersName)";

                var cmd = new SqlCommand(text, connection);

                cmd.Parameters.AddWithValue("@Temperature", WeatherData.main.temp);
                cmd.Parameters.AddWithValue("@CurrentConditions", WeatherData.weather.First().description);
                cmd.Parameters.AddWithValue("@UsersName", userName);
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        static public List<PastWeatherForecasts> GetCurrentTempAndCond(SqlConnection connection)
        {
            var currentweather = new List<PastWeatherForecasts>();
            var sqlCommand = new SqlCommand(@"SELECT Temperature, CurrentConditions FROM Forecast", connection);

            connection.Open();
            var reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                var weather = new PastWeatherForecasts(reader);
                currentweather.Add(weather);
            }
            connection.Close();
            return currentweather;
        }


        static void Main(string[] args)
        {
            var name = RequestUserName();
            var weather = GetWeather();
            AddForecast(weather, name);
            Console.ReadLine();



        }
    }
}
