using Caveman.Setting;
using Newtonsoft.Json;

namespace Caveman.Specification
{
    [JsonObject(MemberSerialization.OptIn)]
    public class BonusSpecification : ISettings
    {
        public BonusSpecification(string name, int timeRespawn, float duration, int maxCountOnMap)
        {
            this.name = name;
            this.timeRespawn = timeRespawn;
            this.duration = duration;
            this.maxCountOnMap = maxCountOnMap;
        }

        [JsonProperty] private readonly string name;
        [JsonProperty] private readonly int timeRespawn;
        [JsonProperty] private readonly float duration;
        [JsonProperty] private readonly int maxCountOnMap;
        [JsonProperty] private readonly Types type;

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

        public int MaxCountOnMap
        {
            get { return maxCountOnMap; }
        }

        public Types Type
        {
            get { return type; }
        }

        public enum Types
        {
            Speed,
            Shield
        }
    }
}
