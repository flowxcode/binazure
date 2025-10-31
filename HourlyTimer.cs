using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using NCrontab;

namespace Company.Function
{
    public class HourlyTimer
    {
        private static readonly CrontabSchedule Schedule = CrontabSchedule.Parse(
            "0 * * * * *", 
            new CrontabSchedule.ParseOptions { IncludingSeconds = true });

        [FunctionName("HourlyTimer")]
        public void Run([TimerTrigger("0 * * * * *")] TimerInfo myTimer, ILogger log)
        {
            var now = DateTime.UtcNow;
            var nextAccurate = Schedule.GetNextOccurrence(now);

            log.LogInformation($"Timer fired at: {now:HH:mm:ss}");
            log.LogInformation($"Next run at: {nextAccurate:HH:mm:ss} (accurate)");
            
            log.LogInformation("Processing data...");
            int result = SomeSimpleCalculation();
            log.LogInformation($"Result: {result}");

            // Optional: ignore built-in Next
            // log.LogInformation($"Built-in Next (may be current): {myTimer.ScheduleStatus.Next}");
        }

        private int SomeSimpleCalculation()
        {
            return 42;
        }
    }
}