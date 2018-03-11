using System;
using System.Net.Http;
using Microsoft.Owin.Hosting;

namespace WAES.WebApi.SelfHost
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            const string baseAddress = "http://localhost:9000/";

            // Start OWIN host 
            using (WebApp.Start<Startup>(baseAddress))
            {
                Console.WriteLine("API is allive on {0}", baseAddress);
                Console.WriteLine("Press Enter to Exit.");
                Console.ReadLine();
            }
        }
    }
}