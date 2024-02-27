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

    public string Rank { get; set; }
    public string Similarity { get; set; }

    public void OnGet(string id)
    {
        IDatabase _db = _redis.GetDatabase();
        _logger.LogDebug(id);

        //TODO: проинициализировать свойства Rank и Similarity значениями из БД
        Rank = _db.StringGet($"RANK-{id}");
        Similarity = _db.StringGet($"SIMILARITY-{id}");

        /*if (rankValue != null)
        {
            Double num;
            if (Double.TryParse(rankValue, out num))
            {
                Rank = num;
            }
        }

        if (similarityValue != null)
        {
            Similarity = Convert.ToDouble(similarityValue);
            *//*Double num;
            if (Double.TryParse(rankValue, out num))
            {
                Rank = num;
            }*//*
        }
*/
    }
}
