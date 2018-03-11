using System;
using System.Net.Http;
using Microsoft.Owin.Hosting;

namespace WAES.WebApi.SelfHost
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var baseAddress = "http://localhost:9000/";

            // Start OWIN host 
            using (WebApp.Start<Startup>(baseAddress))
            {
                //todo add proper comment
                // Create HttpCient and make a request to api/values 
                //var client = new HttpClient();

                //var response = client.GetAsync(baseAddress + "v1/diff/1").Result;

                //Console.WriteLine(response);
                //Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                Console.ReadLine();
            }
        }
    }
}