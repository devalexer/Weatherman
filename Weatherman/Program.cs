using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
            string userName = Console.ReadLine();

            return userName;
        }

        static string RequestUserZip()
        {
            Console.WriteLine("Please enter your zip code: ");
            string userZip = Console.ReadLine();

            return userZip;
        }

        static void GetWeather()
        {
            var url = $"http://api.openweathermap.org/data/2.5/weather?zip={RequestUserZip()},us&units=metric&appid=bcf7aaa93424ebce8079e123df0cf3b9";
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
        }
        

        static void Main(string[] args)
        {
            RequestUserName();
            GetWeather();

        }
    }
}
