using System.Text.RegularExpressions;

namespace tsgsBot_C_.Utils
{
    public static class HelperMethods
    {
        public static long? ParseDuration(string? duration)
        {
            if (string.IsNullOrWhiteSpace(duration))
                return null;

            Match match = Regex.Match(duration, @"^(\d+)([smhd])$", RegexOptions.IgnoreCase);
            if (!match.Success)
                return null;

            if (!long.TryParse(match.Groups[1].Value, out long val))
                return null;

            return match.Groups[2].Value.ToLower() switch
            {
                "s" => val * 1000,
                "m" => val * 60 * 1000,
                "h" => val * 3600 * 1000,
                "d" => val * 86400 * 1000,
                _ => null
            };
        }
    }
}
