using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StackExchange.Redis;

namespace Valuator.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _db;

    public IndexModel(ILogger<IndexModel> logger, IConnectionMultiplexer redis)
    {
        _logger = logger;
        _redis = redis;
        _db = _redis.GetDatabase();
    }

    public void OnGet()
    {

    }

    public IActionResult OnPost(string text)
    {
        if (text == "")
        {
            return Redirect($"index");
        }
        _logger.LogDebug(text);

        string id = Guid.NewGuid().ToString();

        string textKey = "TEXT-" + id;
        string similarityKey = "SIMILARITY-" + id;
        //TODO: посчитать similarity и сохранить в БД по ключу similarityKey
        _db.StringSet(similarityKey, CalculateSimilarity(textKey, text));

        //TODO: сохранить в БД text по ключу textKey
        _db.StringSet(text, textKey);

        string rankKey = "RANK-" + id;
        //TODO: посчитать rank и сохранить в БД по ключу rankKey
        _db.StringSet(rankKey, CalculateRank(text));

        return Redirect($"summary?id={id}");
    }

    private static double CalculateRank(string text)
    {
        int nonAlphabeticCount = 0;
        int totalCharacterCount = text.Length;

        foreach (char c in text)
        {
            if (!char.IsLetter(c))
            {
                nonAlphabeticCount++;
            }
        }

        double rank = (double)nonAlphabeticCount / (double)totalCharacterCount;

        return rank;
    }

    private int CalculateSimilarity(string textKey, string text)
    {
        string? _textKey = _db.StringGet(text);

        if (_textKey == null)
        {
            return 0;
        }

        return 1;
    }
}

