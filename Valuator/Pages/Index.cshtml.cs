using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StackExchange.Redis;
using NATS.Client;
using System.Text;

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
        if (string.IsNullOrEmpty(text))
        {
            return Redirect($"index");
        }
        _logger.LogDebug(text);

        string id = Guid.NewGuid().ToString();

        string textKey = "TEXT-" + id;
        string similarityKey = "SIMILARITY-" + id;

        _db.StringSet(similarityKey, CalculateSimilarity(textKey, text).ToString());
        _db.StringSet(text, textKey);

        SendToRankCalculate(id);

        return Redirect($"summary?id={id}");
    }

    private static void SendToRankCalculate(string id)
    {
        CancellationTokenSource cts = new CancellationTokenSource();

        ConnectionFactory cf = new ConnectionFactory();
        Options opts = ConnectionFactory.GetDefaultOptions();
        opts.Url = "nats://localhost:4444";

        IConnection c = cf.CreateConnection(opts);

        c.Publish("rank", Encoding.UTF8.GetBytes(id));

        c.Drain();

        c.Close();

        cts.Cancel();
    }


    private double CalculateSimilarity(string textKey, string text)
    {
        string? _textKey = _db.StringGet(text);

        if (_textKey == null)
        {
            return 0;
        }

        return 1;
    }
}

