using JokeGenerator;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace ConsoleApp1
{
    class JsonFeed
    {
        static string _url = "";
        static int count = 0;
        private static IHttpClientFactory _clientFactory;

        public JsonFeed() { }
        public JsonFeed(string endpoint, int results, IHttpClientFactory clientfactory)
        {
            _url = endpoint;
            count = results;
            _clientFactory = clientfactory;
        }
        /// <summary>
        /// calls 'jokes/random' api
        /// </summary>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <param name="category"></param>
        /// <returns>list of random jokes</returns>
        public static List<string> GetRandomJokes(string firstname, string lastname, string category)
        {
            try
            {
                List<string> lstJokes = new List<string>();

                var client = GetClient();
                client.BaseAddress = new Uri(_url);
                string url = "jokes/random";

                var uriBuilder = new UriBuilder(client.BaseAddress + url);
                var parameters = HttpUtility.ParseQueryString(uriBuilder.Query);
                if (category != null)
                    parameters["category"] = category;
                uriBuilder.Query = parameters.ToString();

                string response = Task.FromResult(client.GetStringAsync(uriBuilder.ToString()).Result).Result;
                if (firstname != null && lastname != null)
                {
                    int index = response.IndexOf("Chuck Norris");
                    string firstPart = response.Substring(0, index);
                    string secondPart = response.Substring(0 + index + "Chuck Norris".Length, response.Length - (index + "Chuck Norris".Length));
                    response = firstPart + " " + firstname + " " + lastname + secondPart;
                }
                var result = JsonConvert.DeserializeObject<Joke>(response).value;

                lstJokes.Add(result);
                return lstJokes;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured" + ex.Message);
                return null;
            }
            
        }

        /// <summary>
        /// returns an object that contains name and surname
        /// </summary>
        /// <param name="client2"></param>
        /// <returns></returns>
		public static Names Getnames()
        {
            try
            {
                var client = GetClient();
                client.BaseAddress = new Uri(_url);
                var result = client.GetStringAsync("").Result;
                return JsonConvert.DeserializeObject<Names>(result);
            }            
            catch (Exception ex)
            {
                Console.WriteLine("Error occured" + ex.Message);
                return null;
            }
        }
        /// <summary>
        /// calls '/jokes/categories' api 
        /// </summary>
        /// <returns> categories </returns>
        public static async Task<string[]> GetCategories()
        {
            try
            {
                var client = GetClient();
                client.BaseAddress = new Uri(_url);
                var path = client.BaseAddress + "/jokes/categories";
                return new string[] { Task.FromResult(client.GetStringAsync(path).Result).Result };
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error occured" + ex.Message);
                return null;
            }           
        }
        /// <summary>
        /// Creates HttpClient
        /// </summary>
        /// <returns></returns>
        public static HttpClient GetClient()
        {            
            return _clientFactory.CreateClient(Options.DefaultName);
        }
    }
}
