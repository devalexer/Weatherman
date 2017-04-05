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
        
        //static string RequestUserInput()
        //{
        //    Console.WriteLine("Please enter your name: ");
        //    string userName = Console.ReadLine();
        //    Console.WriteLine("Please enter your zip code: ");
        //    string userZip = Console.ReadLine();

        //    return userName;
        //    return userZip;
        //}


        static void Main(string[] args)
        {
            int userZip = 65803;
            var url = $"api.openweathermap.org/data/2.5/weather?zip={userZip},us&APPID=bcf7aaa93424ebce8079e123df0cf3b9"; 
            var request = WebRequest.Create(url);
            var response = request.GetResponse();
            var rawResponse = String.Empty;

            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                rawResponse = reader.ReadToEnd();
                Console.WriteLine(rawResponse);
            }

            var zip = JsonConvert.DeserializeObject<Character>(rawResponse);

            Console.WriteLine();
            
        }
    }
}
