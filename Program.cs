using System;

namespace MovieRecommender
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string moviesFileName = @"Data/imdb_top_1000.csv";
            string platformsFileName = @"data/MoviesOnStreamingPlatforms.csv";
            string conjPrepFileName = @"data/Conjunctions_and_Prepositions.txt";

            App app = new App(moviesFileName, platformsFileName, conjPrepFileName);
            
        }
    }
}