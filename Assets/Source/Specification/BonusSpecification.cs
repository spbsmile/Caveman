using Caveman.Setting;
using System.Runtime.Serialization;

namespace Caveman.Specification
{
    [DataContract]
    public class BonusSpecification : ISettings
    {
        public BonusSpecification(string name, int timeRespawn, float duration, int maxCountOnMap, Types type)
        {
            this.name = name;
            this.timeRespawn = timeRespawn;
            this.duration = duration;
            this.maxCountOnMap = maxCountOnMap;
            this.type = type;
        }

        [DataMember] private readonly string name;
        [DataMember] private readonly int timeRespawn;
        [DataMember] private readonly float duration;
        [DataMember] private readonly int maxCountOnMap;
        [DataMember] private readonly Types type;

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
