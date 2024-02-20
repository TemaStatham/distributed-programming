using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Valuator.Pages;
public class SummaryModel : PageModel
{
    private readonly ILogger<SummaryModel> _logger;
    private readonly IConnectionMultiplexer _redis;
    // private readonly IDatabase _db;
    public SummaryModel(ILogger<SummaryModel> logger, IConnectionMultiplexer redis)
    {
        _logger = logger;
        _redis = redis;
    }

    public double Rank { get; set; }
    public double Similarity { get; set; }

    public void OnGet(string id)
    {
        IDatabase _db = _redis.GetDatabase();
        _logger.LogDebug(id);

        //TODO: проинициализировать свойства Rank и Similarity значениями из БД
        string? rankValue = _db.StringGet($"RANK-{id}");
        string? similarityValue = _db.StringGet($"SIMILARITY-{id}");

        if (rankValue != null)
        {
            Rank = Convert.ToDouble(rankValue);
        }

        if (similarityValue != null)
        {
            Similarity = Convert.ToDouble(similarityValue);
        }

    }
}
