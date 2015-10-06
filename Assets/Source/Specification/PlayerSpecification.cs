using Caveman.Setting;
using Newtonsoft.Json;

namespace Caveman.Specification
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PlayerSpecification : ISettings
    {
        public PlayerSpecification(string name, float speed, int timeRespawn, int timeInvulnerability, Types type)
        {
            this.name = name;
            this.speed = speed;
            this.timeRespawn = timeRespawn;
            this.timeInvulnerability = timeInvulnerability;
            this.type = type;
        }

        [JsonProperty] private readonly string name;
        [JsonProperty] private float speed;
        [JsonProperty] private readonly int timeRespawn;
        [JsonProperty] private readonly int timeInvulnerability;
        [JsonProperty] private readonly Types type;

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

        public void SetSpeed(float value)
        {
            speed = value;
        }
        
        public enum Types
        {
            Sample
        }
    }
}
