using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using NCrontab;

namespace Company.Function
{
    public class HourlyTimer
    {
        private readonly ILogger<HourlyTimer> _logger;
        private static readonly CrontabSchedule Schedule = CrontabSchedule.Parse(
            "0 * * * * *", new CrontabSchedule.ParseOptions { IncludingSeconds = true });

        public HourlyTimer(ILogger<HourlyTimer> logger) => _logger = logger;

        [Function("HourlyTimer")]
        public async Task Run([TimerTrigger("0 * * * * *")] TimerInfo timer)
        {
            var now = DateTime.UtcNow;
            var next = Schedule.GetNextOccurrence(now);

            _logger.LogInformation($"Timer fired at: {now:HH:mm:ss}");
            _logger.LogInformation($"Next run at: {next:HH:mm:ss} (accurate)");

            _logger.LogInformation("Processing data...");
            int result = SomeSimpleCalculation();
            _logger.LogInformation($"Result: {result}");

            // ---- OPTIONAL: Binance price (no key needed) ----
            // var price = await GetBinancePriceAsync();
            // _logger.LogInformation($"BTC/USDT price: ${price}");
        }

        private int SomeSimpleCalculation() => 42;

        // Uncomment if you want a quick Binance price check
        // private static async Task<string> GetBinancePriceAsync()
        // {
        //     using var client = new System.Net.Http.HttpClient();
        //     var json = await client.GetStringAsync(
        //         "https://api.binance.com/api/v3/ticker/price?symbol=BTCUSDT");
        //     var doc = System.Text.Json.JsonDocument.Parse(json);
        //     return doc.RootElement.GetProperty("price").GetString()!;
        // }
    }
}