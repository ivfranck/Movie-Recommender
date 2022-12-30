namespace MovieRecommender;

public class App
{
    private Dictionary<string, List<List<string>>> Movies { get; set; }
    private Dictionary<string, List<List<string>>> Platforms { get; set; }
    private List<string> Excl { get; set; }
    
    public App(string moviesFileName, string platformsFileName, string conjPrepFile)
    {
        Movies = new Dictionary<string, List<List<string>>>();
        Platforms = new Dictionary<string, List<List<string>>>();
        Excl = new List<string>();
        Preprocessing( moviesFileName, platformsFileName, conjPrepFile);
    }

    public void Preprocessing(string moviesFileName, string platformsFileName, string conjPrepFile)
    {
        string[] moviesFile = File.ReadAllLines(moviesFileName);
        string[] platformsFile = File.ReadAllLines(platformsFileName);
        string[] eng = File.ReadAllLines(conjPrepFile);
        eng = Array.ConvertAll(eng, d => d.ToLower());
        Excl = eng.ToList();
        
        
        foreach (var ele in moviesFile)
        {
            string[] str = ele.Split(";");
            // title: str[0] year: str[1] genre: str[4] rating: str[5] overview: str[6] metaScore: str[7]
            // director: str[8] star1: str[9] star2: str[10] star3: str[11] star4: str[12]
            
            if (!Movies.ContainsKey(str[0]))
            {
                Movies.Add(str[0],
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
            string[] str = ele.Split(";");
            if (Movies.ContainsKey(str[0]))
            {
                Platforms.Add(str[0],
                    new List<List<string>>
                    {
                        new List<string> { str[1] },
                        new List<string> { str[2] },
                        new List<string> { str[3] },
                        new List<string> { str[4] }
                    });
            }
        }
        
        /*foreach (KeyValuePair<string, List<List<string>>> kvp in Platforms)
        {
            Console.WriteLine(kvp.Key);
            
        }*/

       
    }

}