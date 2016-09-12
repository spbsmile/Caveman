using Caveman.Setting;
using Newtonsoft.Json;

namespace Caveman.Configs.Levels
{
    [JsonObject(MemberSerialization.OptIn)]
    public class MultiplayerLevelConfig : ISettings
    {
        public MultiplayerLevelConfig(string name, int roundTime)
        {
            this.roundTime = roundTime;
            this.name = name;
        }

        [JsonProperty] private readonly string name;
        [JsonProperty] private readonly int roundTime;

        public string Name
        {
            get { return name; }
        }

        public int RoundTime
        {
            get { return roundTime; }
        }
    }
}