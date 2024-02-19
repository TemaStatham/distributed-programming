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
    private readonly ConnectionMultiplexer _redis;
    private readonly IDatabase _db;
    public SummaryModel(ILogger<SummaryModel> logger, ConnectionMultiplexer redis)
    {
        _logger = logger;
        _redis = redis;
        _db = _redis.GetDatabase();
    }

    public double Rank { get; set; }
    public double Similarity { get; set; }

    public void OnGet(string id)
    {
        _logger.LogDebug(id);

        //TODO: проинициализировать свойства Rank и Similarity значениями из БД
        string? rankValue = _db.StringGet($"RANK-{id}");
        string? similarityValue = _db.StringGet($"SIMILARITY-{id}");

        if (rankValue != null)
        {
            Rank = double.Parse(rankValue);
        }

        if (similarityValue != null)
        {
            Similarity = double.Parse(similarityValue);
        }

    }
}
