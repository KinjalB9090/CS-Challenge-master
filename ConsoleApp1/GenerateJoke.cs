using ConsoleApp1;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace JokeGenerator
{
    public class GenerateJoke
    {
        static string[] results = new string[50];
        static List<string> randomJoke = new List<string>();
        static char key;
        static Tuple<string, string> names;
        //static ConsolePrinter printer = new ConsolePrinter();
        static IHttpClientFactory httpClientFactory;

        public static async Task GetJokes()
        {
            SetProvider();

            Print("\n*********************   Welcome to the Joke Generator!  ********************** \n");
            Print("Press ? to get instructions");
            if (Console.ReadLine() == "?")
            {
                while (true)
                {
                    Print("Press c to get categories");
                    Print("Press r to get random jokes");
                    Print("---------------------------------------------------------");

                    await getCategories();

                    GetEnteredKey(Console.ReadKey());
                    if (key == 'c')
                    {
                        Print("\n****--------------- Categories -----------------****");                      
                        Print($"Categories : \n {string.Join(",", results)}");
                    }
                    if (key == 'r')
                    {
                        Print("\nWant to use a random name? y/n");
                        GetEnteredKey(Console.ReadKey());
                        if (key == 'y')
                            GetNames();

                        Print("\nWant to specify a category? y/n");
                        GetEnteredKey(Console.ReadKey());
                        if (key == 'y')
                        {
                            Print("\nEnter a category from the below options");
                            Print("\n****--------------- Categories -----------------****\n");
                            Print($"{ string.Join(",", results) }");
                            var category = Console.ReadLine().ToString();

                            Print("How many jokes do you want? (1-9)");
                            int jokeCount = ReadIntInput();

                            GetSpecifiedJokes(category, jokeCount);
                        }
                        else
                        {
                            Print("\nHow many jokes do you want? (1-9)");
                            int jokeCount = ReadIntInput();
                            GetSpecifiedJokes(null, jokeCount);
                        }
                    }
                    names = null;
                }
            }          
        }

        private static void GetSpecifiedJokes(string category, int jokeCount)
        {
            Print("\n************************ Started Generating Jokes ****************************");
            for (int x = 0; x < jokeCount; x++)
            {
                GetRandomJokes(category, jokeCount);
                Print("\n---------------------------------------------------------\n");
                Print($"Joke {x + 1} : {string.Join(",", randomJoke)}");
            }
            Print("\n************************  Generation ended ****************************");
        }

        private static int ReadIntInput()
        {
            int readKey = 0;
            Int32.TryParse(Console.ReadLine(), out readKey);
            return readKey;
        }
        private static void Print(string strPrint)
        {
            Console.WriteLine(strPrint);
        }
        private static void SetProvider()
        {
            var serviceProvider = new ServiceCollection().AddHttpClient().BuildServiceProvider();
            httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
        }
        
        private static void GetEnteredKey(ConsoleKeyInfo consoleKeyInfo)
        {
            switch (consoleKeyInfo.Key)
            {
                case ConsoleKey.C:
                    key = 'c';
                    break;
                case ConsoleKey.D0:
                    key = '0';
                    break;
                case ConsoleKey.D1:
                    key = '1';
                    break;
                case ConsoleKey.D3:
                    key = '3';
                    break;
                case ConsoleKey.D4:
                    key = '4';
                    break;
                case ConsoleKey.D5:
                    key = '5';
                    break;
                case ConsoleKey.D6:
                    key = '6';
                    break;
                case ConsoleKey.D7:
                    key = '7';
                    break;
                case ConsoleKey.D8:
                    key = '8';
                    break;
                case ConsoleKey.D9:
                    key = '9';
                    break;
                case ConsoleKey.R:
                    key = 'r';
                    break;
                case ConsoleKey.Y:
                    key = 'y';
                    break;
                case ConsoleKey.N:
                    key = 'n';
                    break;
            }
        }

        private static void GetRandomJokes(string category, int number)
        {
            try {
                new JsonFeed("https://api.chucknorris.io", number, httpClientFactory);
                randomJoke = JsonFeed.GetRandomJokes(names?.Item1, names?.Item2, category).ToList();
            }           
            catch (Exception ex)
            {
                Console.WriteLine("Error occured" + ex.Message);               
            }
        }

        private static async Task getCategories()
        {
            try
            {
                new JsonFeed("https://api.chucknorris.io", 0, httpClientFactory);
                results = await JsonFeed.GetCategories();
            }          
            catch (Exception ex)
            {
                Console.WriteLine("Error occured" + ex.Message);
            }
        }
        private static void GetNames()
        {
            try
            {
                new JsonFeed("https://www.names.privserv.com/api/", 0, httpClientFactory);
                Names result = JsonFeed.Getnames();
                names = Tuple.Create(result.Name.ToString(), result.Surname.ToString());
            }          
             catch (Exception ex)
            {
                Console.WriteLine("Error occured" + ex.Message);
            }

        }
    }
}
