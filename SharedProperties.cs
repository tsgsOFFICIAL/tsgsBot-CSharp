namespace tsgsBot_C_
{
    public sealed class SharedProperties
    {
        private static readonly Lazy<SharedProperties> _instance = new(() => new SharedProperties());
        public static SharedProperties Instance => _instance.Value;

        private static DateTimeOffset _uptime = DateTimeOffset.UtcNow;
        public DateTimeOffset UpTime
        {
            get => _uptime;
            set => _uptime = value;
        }

        public string STEAM_WEB_API_KEY { get; private set; } = string.Empty;
        public string STEAM_API_KEY { get; private set; } = string.Empty;
        public ulong GuildId { get; private set; } = 227048721710317569;
        public ulong MemberChannelId = 604739962629521418;
        public ulong BotChannelId = 604739965124870164;
        public ulong CombinedChannelId = 604739960146493441;

        //public ulong GuildId { get; private set; } = 1120646718132867132;
        //public ulong MemberChannelId = 1120646719269511212;
        //public ulong BotChannelId = 1120646719269511213;
        //public ulong CombinedChannelId = 1461285047704227850;

        private SharedProperties()
        {
            UpTime = DateTimeOffset.UtcNow;
            STEAM_API_KEY = Environment.GetEnvironmentVariable("STEAM_API_KEY") ?? string.Empty;
            STEAM_WEB_API_KEY = Environment.GetEnvironmentVariable("STEAM_WEB_API_KEY") ?? string.Empty;
        }

        public void Initialize()
        {
            // Intentionally left blank
        }
    }
}
