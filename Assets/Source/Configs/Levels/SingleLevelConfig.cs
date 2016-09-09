using System.Collections.Generic;
using Caveman.Setting;
using Newtonsoft.Json;

namespace Caveman.Configs.Levels
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SingleLevelConfig : ISettings
    {
        public SingleLevelConfig(string name, int roundTime, int botsCount, List<string> botsName)
        {
            this.name = name;
            this.roundTime = roundTime;
            this.botsCount = botsCount;
            this.botsName = botsName;
        }

        [JsonProperty] private readonly string name;
        [JsonProperty] private readonly int roundTime;
        [JsonProperty] private readonly int botsCount;
        [JsonProperty] private readonly List<string> botsName;

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

        public List<string> BotsName
        {
            get { return botsName; }
        }

    }
}