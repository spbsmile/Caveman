using Caveman.Setting;
using Newtonsoft.Json;

namespace Caveman.Configs
{
    [JsonObject(MemberSerialization.OptIn)]
    public class BonusConfig : ISettings
    {
        public BonusConfig(string name, float duration, Types type, string prefabPath)
        {
            this.name = name;
            this.duration = duration;
            this.type = type;
            this.prefabPath = prefabPath;
        }

        [JsonProperty] private readonly string name;
        [JsonProperty] private readonly int timeRespawn;
        [JsonProperty] private readonly float duration;
        [JsonProperty] private readonly Types type;
        [JsonProperty] private string prefabPath;

        public string Name
        {
            get { return name; }
        }

        public int TimeRespawn
        {
            get { return timeRespawn; }
        }

        public float Duration
        {
            get { return duration; }
        }

        public Types Type
        {
            get { return type; }
        }

        public string PrefabPath
        {
            get { return prefabPath; }
        }

        public enum Types
        {
            Speed,
            Shield
        }
    }
}
