using System;

namespace MovieRecommender
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            const string moviesFileName = @"Data/imdb_top_1000.csv";
            const string platformsFileName = @"data/MoviesOnStreamingPlatforms.csv";
            const string conjPrepFileName = @"data/Conjunctions_and_Prepositions.txt";
            
            Console.WriteLine("Search movie:");
            var input = Console.ReadLine();
            while (!string.IsNullOrEmpty(input))
            {
                try
                {
                    var app = new App(input.Trim(), moviesFileName, platformsFileName, conjPrepFileName);
                    Console.WriteLine(app.IsMovie());
                    Console.WriteLine("\nSearch movie or press enter to exit");
                }
                catch (Exception e)
                {
                    Console.WriteLine("An error occured. Please try a different search... :)");
                }

                input = Console.ReadLine();
            }
        }
    }
}