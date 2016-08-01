using Caveman.Setting;
using Newtonsoft.Json;

namespace Caveman.Configs.Levels
{
    [JsonObject(MemberSerialization.OptIn)]
    public class LevelMultiplayerConfig : ISettings
    {
        public LevelMultiplayerConfig(string name, int roundTime)
        {
            this.roundTime = roundTime;
            this.name = name;
        }

        [JsonProperty] private string name;
        [JsonProperty] private int roundTime;

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