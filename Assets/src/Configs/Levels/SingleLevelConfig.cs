using Newtonsoft.Json;

namespace Caveman.Configs.Levels
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SingleLevelConfig : IConfig
    {
        public SingleLevelConfig(string name, int roundTime, int botsCount, string[] botsName)
        {
            this.name = name;
            this.roundTime = roundTime;
            this.botsCount = botsCount;
            this.botsName = botsName;
        }

        [JsonProperty] private readonly string name;
        [JsonProperty]
        private readonly int roundTime;
        [JsonProperty]
        private readonly int botsCount;
        [JsonProperty]
        private readonly string[] botsName;

        public string Name
        {
            get { return name; }
        }

        public int RoundTime
        {
            get { return roundTime; }
        }

        public int BotsCount
        {
            get { return botsCount; }
        }

        public string[] BotsName
        {
            get { return botsName; }
        }
    }
}