namespace MovieRecommender;

public class CosineSimilarity
{
    private readonly string _compText;
    public CosineSimilarity(string text)
    {
        _compText = text;
    }

    private static double DotProduct(List<int> vectorA, List<int> vectorB)
    {
        double product = 0;

        for (int i = 0; i < vectorA.Count; i++)
        {
            product += vectorA[i] * vectorB[i];
        }

        return product;
    }

    private static double Magnitude(List<int> vector)
    {
        double sum = 0;

        for (int i = 0; i < vector.Count; i++)
        {
            sum += vector[i] * vector[i];
        }

        return Math.Sqrt(sum);
    }

    private static double CosSimilarity(List<int> vectorA, List<int> vectorB)
    {
        return DotProduct(vectorA, vectorB) / (Magnitude(vectorA) * Magnitude(vectorB));
    }

    private Dictionary<string, int>  WordCount(string text)
    {
        var wordCount = new Dictionary<string, int>();
        var w = text.ToLower().Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var word in w)
        {
            if (!App.Excl.Contains(word))
            {
                var matchQuery = from ele in w 
                    where ele.Equals(word.ToLower(), StringComparison.InvariantCultureIgnoreCase)  
                    select ele; 
                 
                var counter = matchQuery.Count();
                wordCount[word] = counter;
            }
        }
        return wordCount;
    }

    private static void AddToDict(Dictionary<string, int> wordCount, Dictionary<string, bool> allWords)
    {
        foreach (var kvp in wordCount)
        {
            allWords[kvp.Key] = true;
        }
    }

    private static List<int> CreateVector(Dictionary<string, int> wordCount, Dictionary<string, bool> allWords)
    {
        var vector = new List<int>();

        foreach (var kvp in allWords)
        {
            if (wordCount.ContainsKey(kvp.Key))
            {
                vector.Add(wordCount[kvp.Key]);
            }
            else
            {
                vector.Add(0);
            }
        }

        return vector;
    }

    public double Result()
    {
        // Map words to their frequency count
        var wordCountA = WordCount(App._input);
        var wordCountB = WordCount(_compText);

        var allWords = new Dictionary<string, bool>();
        
        AddToDict(wordCountA, allWords);
        AddToDict(wordCountB, allWords);

        var vectorA = CreateVector(wordCountA, allWords);
        var vectorB = CreateVector(wordCountB, allWords);

        return CosSimilarity(vectorA, vectorB);
    }
}