using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

namespace Boilerplate.TestConsole
{
    /// <summary>
    /// Requests a token from our OAuth server and uses response to call our API
    /// </summary>
    internal class Program
    {
        public static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        private static async Task MainAsync()
        {
            Console.Title = "Console";

            // Get endpoints
            var discoveryResponse = await DiscoveryClient.GetAsync("http://localhost:5000");

            // Request token
            var tokenClient = new TokenClient(discoveryResponse.TokenEndpoint, "consoleapptoken", "super-secret-key");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("oauth-boilerplate");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                Console.ReadKey();
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n");

            // Call API using access token from our OAuth server
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var response = await client.GetAsync("http://localhost:5001/identity");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }

            Console.ReadKey();
        }
    }
}
