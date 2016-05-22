using System.Runtime.Serialization;
using Caveman.Setting;

namespace Caveman.Specification
{
    [DataContract]
    public class BonusSpecification : ISettings
    {
        public BonusSpecification(string name, float duration, Types type)
        {
            this.name = name;
            this.duration = duration;
            this.type = type;
        }

        [DataMember] private readonly string name;
        [DataMember] private readonly int timeRespawn;
        [DataMember] private readonly float duration;
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
