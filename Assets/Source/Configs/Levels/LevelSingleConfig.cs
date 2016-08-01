using Caveman.Setting;
using Newtonsoft.Json;

namespace Caveman.Configs.Levels
{
    [JsonObject(MemberSerialization.OptIn)]
    public class LevelSingleConfig : ISettings
    {
        public LevelSingleConfig(string name, int roundTime, int botsCount, string[] botsName)
        {
            this.name = name;
            this.roundTime = roundTime;
            this.botsCount = botsCount;
            this.botsName = botsName;
        }

        [JsonProperty] private string name;
        [JsonProperty] private int roundTime;
        [JsonProperty] private int botsCount;
        [JsonProperty] private string[] botsName;

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