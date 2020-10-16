using JokeGenerator;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {  
        static async Task MainAsync(string[] args)
        {  
            await Task.Run(async () => await GenerateJoke.GetJokes());            
        }
        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();  
        } 
    }
}
