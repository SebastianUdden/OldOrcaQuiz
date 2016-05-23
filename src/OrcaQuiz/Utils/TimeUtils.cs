using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrcaQuiz.Utils
{
    public static class TimeUtils
    {
        public static int? GetSecondsLeft(int? timeLimitInMinutes, DateTime StartTime)
        {
            if (!timeLimitInMinutes.HasValue)
                return null;
            var timeLimit = TimeSpan.FromMinutes(timeLimitInMinutes.Value);
            var timeLeft = timeLimit - (DateTime.UtcNow - StartTime);
            return ((int) timeLeft.TotalSeconds);
        }

        public static bool HasTimeLeft(int? timeLimitInMinutes, DateTime StartTime)
        {
            if (!timeLimitInMinutes.HasValue)
                return true;            
            return (GetSecondsLeft(timeLimitInMinutes, StartTime) > 0);
        }
         
    }
}
