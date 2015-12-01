using Caveman.Setting;
using System.Runtime.Serialization;

namespace Caveman.Specification
{
    [DataContract]
    public class PlayerSpecification : ISettings
    {
        public PlayerSpecification(string name, float speed, int timeRespawn, int timeInvulnerability, Types type, int gold)
        {
            this.name = name;
            this.speed = speed;
            this.timeRespawn = timeRespawn;
            this.timeInvulnerability = timeInvulnerability;
            this.type = type;
            this.gold = gold;
        }

        [DataMember] private readonly string name;
        [DataMember] private readonly float speed;
        [DataMember] private readonly int timeRespawn;
        [DataMember] private readonly int timeInvulnerability;
        [DataMember] private readonly Types type;
        [DataMember] private readonly int gold;

        public string Name
        {
            get { return name; }
        }

        public float Speed
        {
            get { return speed; }
        }

        public int TimeRespawn
        {
            get { return timeRespawn; }
        }

        public int TimeInvulnerability
        {
            get { return timeInvulnerability; }
        }

        public Types Type
        {
            get { return type; }
        }

        public int Gold
        {
            get { return gold; }
        }

        public enum Types
        {
            Sample
        }
    }
}
