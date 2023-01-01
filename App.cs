namespace MovieRecommender;

public class App
{
    private static Dictionary<string, List<List<string>>> Movies { get; set; }
    private Dictionary<string, List<List<string>>> Platforms { get; set; }
    public static List<string> Excl { get; private set; }
    public static string _input { get; private set; }
    
    public App(string inputStr, string moviesFileName, string platformsFileName, string conjPrepFile)
    {
        Movies = new Dictionary<string, List<List<string>>>();
        Platforms = new Dictionary<string, List<List<string>>>();
        Excl = new List<string>();
        _input = inputStr;
        Preprocessing( moviesFileName, platformsFileName, conjPrepFile);
    }

    private void Preprocessing(string moviesFileName, string platformsFileName, string conjPrepFile)
    {
        var moviesFile = File.ReadAllLines(moviesFileName);
        var platformsFile = File.ReadAllLines(platformsFileName);
        var eng = File.ReadAllLines(conjPrepFile);
        eng = Array.ConvertAll(eng, d => d.ToLower());
        Excl = eng.ToList();
        
        
        foreach (var ele in moviesFile)
        {
            var str = ele.Split(";");
            
            // title: str[0] year: str[1] genre: str[4] rating: str[5] overview: str[6] metaScore: str[7]
            // director: str[8] star1: str[9] star2: str[10] star3: str[11] star4: str[12]
            
            if (!Movies.ContainsKey(str[0].Trim()))
            {
                Movies.Add(str[0].Trim(),
                    new List<List<string>>
                    {
                        new List<string> { str[1] },
                        new List<string> { str[4] },
                        new List<string> { str[5] },
                        new List<string> { str[6] },
                        new List<string> { str[7] },
                        new List<string> { str[8] },
                        new List<string> { str[9] },
                        new List<string> { str[10] },
                        new List<string> { str[11] },
                        new List<string> { str[12] }
                    });
            }
        }

        foreach (var ele in platformsFile)
        {
            var str = ele.Split(";");
            if (Movies.ContainsKey(str[0].Trim()))
            {
                Platforms.Add(str[0].Trim(),
                    new List<List<string>>
                    {
                        new List<string> { str[1] },
                        new List<string> { str[2] },
                        new List<string> { str[3] },
                        new List<string> { str[4] }
                    });
            }
        }
        
    }

    private string Match()
    {
        var compString = "";
        double smallest = int.MaxValue;
        var movieName = "";
        
        foreach (var kvp in Movies)
        {
            // Concatenating descriptive features 
            compString += kvp.Value[0][0] + " " + kvp.Value[1][0] + " " + kvp.Value[3][0] + " " + kvp.Value[5][0] +
                          " " + kvp.Value[6][0] + " " + kvp.Value[7][0] + " " + kvp.Value[8][0] + " " + kvp.Value[9][0];
            
            // Calculate the cosine similarity and distance
            var cs = new CosineSimilarity(compString);
            var distance = 1 - cs.Result();
            
            if (distance < smallest)
            {
                smallest = distance;
                movieName = kvp.Key;
            }
            else if(Math.Abs(distance - smallest) == 0)
            {
                if (movieName.Length > 0)
                {
                    var metaScore1 = Movies[movieName][4][0];
                    var metaScore2 = Movies[kvp.Key][4][0];
                    var imdbRating1 = Convert.ToDouble(Movies[movieName][2][0]);
                    var imdbRating2 = Convert.ToDouble(Movies[kvp.Key][2][0]);
                   
                    // If the metaScore is available
                    if (metaScore1.Length > 0 && metaScore2.Length > 0)
                    {
                        if (Convert.ToInt32(metaScore2) > Convert.ToInt32(metaScore1))
                        { 
                            smallest = distance;
                            movieName = kvp.Key; 
                        }
                    }
                    // Consider instead the imdb rating
                    else
                    {
                        if (imdbRating2 > imdbRating1)
                        { 
                            smallest = distance;
                            movieName = kvp.Key; 
                        }
                    }
                }
            }
            compString = "";
        }
        return StreamPlatforms(movieName);
    }

    public string IsMovie()
    {
        // Check if user typed in a movie name
        foreach (var kvp in Movies)
        {
            if (kvp.Key.ToLower().Contains(_input.ToLower()))
            {
                return StreamPlatforms(kvp.Key);
            }
        }
        return Match();
    }

    private string StreamPlatforms(string movieName)
    {
        var availablePlatforms = new List<string>();
        
        // If movie name exists in platforms dict
        if (Platforms.ContainsKey(movieName))
        { 
            var netflix = Platforms[movieName][0][0];
            var hulu = Platforms[movieName][1][0];
            var prime = Platforms[movieName][2][0];
            var disney = Platforms[movieName][3][0];
            
            if (Convert.ToInt32(netflix) == 1) { availablePlatforms.Add("Netflix"); }
            if (Convert.ToInt32(hulu) == 1) { availablePlatforms.Add("Hulu"); }
            if (Convert.ToInt32(prime) == 1) { availablePlatforms.Add(" Amazon Prime Video"); }
            if (Convert.ToInt32(disney) == 1) { availablePlatforms.Add("Disney+"); }
            
            var availPlatformsStr = string.Join(", ", availablePlatforms.ToArray());
            
            // If movie streams on a certain platform
            if (availablePlatforms.Count > 0)
            {
                return movieName + "\nWatch it on: " + availPlatformsStr; 
            }
        }
        return movieName + "\nWatch it on: nowhere available";
    }

}