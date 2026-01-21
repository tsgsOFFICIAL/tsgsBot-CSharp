using System.Text.RegularExpressions;

namespace tsgsBot_C_.Utils
{
    public static class HelperMethods
    {
        private static readonly Regex DurationToken = new(@"(\d+)\s*(s|m|h|d|w)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static TimeSpan? ParseDuration(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            MatchCollection matches = DurationToken.Matches(input);
            if (matches.Count == 0)
                return null;

            long totalSeconds = 0;

            foreach (Match m in matches)
            {
                long value = long.Parse(m.Groups[1].Value);
                string unit = m.Groups[2].Value.ToLowerInvariant();

                totalSeconds += unit switch
                {
                    "s" => value,
                    "m" => value * 60,
                    "h" => value * 3600,
                    "d" => value * 86400,
                    "w" => value * 604800,
                    _ => 0
                };
            }

            return TimeSpan.FromSeconds(totalSeconds);
        }

        public static string FormatTimeRemaining(TimeSpan timeSpan)
        {
            if (timeSpan.TotalDays >= 1)
                return $"{(int)timeSpan.TotalDays}d {timeSpan.Hours}h {timeSpan.Minutes}m";
            if (timeSpan.TotalHours >= 1)
                return $"{timeSpan.Hours}h {timeSpan.Minutes}m";
            if (timeSpan.TotalMinutes >= 1)
                return $"{timeSpan.Minutes}m {timeSpan.Seconds}s";
            return $"{timeSpan.Seconds}s";
        }
    }
}