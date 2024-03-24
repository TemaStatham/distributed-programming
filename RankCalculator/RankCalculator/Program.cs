using NATS.Client;
using System.Text;
using StackExchange.Redis;
using System;

namespace RankCalculator
{
    class Program
    {
        private const string NATS_URL = "nats://localhost:4444";
        private const string REDIS_URL = "localhost:6379";
        public static void Main()
        {
            ConfigurationOptions redisConfiguration = ConfigurationOptions.Parse(REDIS_URL);
            ConnectionMultiplexer redisConnection = ConnectionMultiplexer.Connect(redisConfiguration);
            IDatabase db = redisConnection.GetDatabase();

            ConnectionFactory cf = new ConnectionFactory();
            Options opts = ConnectionFactory.GetDefaultOptions();
            opts.Url = NATS_URL;

            IConnection c = cf.CreateConnection(opts);

            EventHandler<MsgHandlerEventArgs> h = (sender, args) =>
            {
                Console.WriteLine($"worker received {args.Message}");
                string id = Encoding.UTF8.GetString(args.Message.Data);

                string textKey = "TEXT-" + id;
                string text = db.StringGet(textKey);

                string rankKey = "RANK-" + id;

                double rank = CalculateRank(text);

                db.StringSet(rankKey, rank);
            };

            IAsyncSubscription s = c.SubscribeAsync("rank", h);

            s.Start();

            Console.WriteLine("AS");
            Console.ReadLine();
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
    }
}
