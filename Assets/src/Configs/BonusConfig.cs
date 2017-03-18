using Caveman.BonusSystem;
using Newtonsoft.Json;

namespace Caveman.Configs
{
    [JsonObject(MemberSerialization.OptIn)]
    public class BonusConfig : IConfig
    {
        public BonusConfig(string name, float duration, BonusType type, string prefabPath, float factor)
        {
            this.name = name;
            this.duration = duration;
            this.type = type;
            this.prefabPath = prefabPath;
            this.factor = factor;
        }

        [JsonProperty] private readonly string name;
        [JsonProperty] private readonly float duration;
        [JsonProperty] private readonly BonusType type;
        [JsonProperty] private readonly string prefabPath;
        [JsonProperty] private readonly float factor;

        public string Name
        {
            get { return name; }
        }

        public float Duration
        {
            get { return duration; }
        }

        public float Factor
        {
            get { return factor; }
        }

        public BonusType Type
        {
            get { return type; }
        }

        public string PrefabPath
        {
            get { return prefabPath; }
        }
    }
}
