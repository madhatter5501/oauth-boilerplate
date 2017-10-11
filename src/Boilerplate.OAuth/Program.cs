using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Boilerplate.OAuth
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "OAuthServer";
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
