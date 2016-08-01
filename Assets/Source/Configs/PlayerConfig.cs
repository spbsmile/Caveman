using Caveman.Setting;
using Newtonsoft.Json;

namespace Caveman.Configs
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PlayerConfig : ISettings
    {
        public PlayerConfig(string name, float speed, int timeRespawn, int timeInvulnerability, float strength,
            Types type, string prefabPath)
        {
            this.name = name;
            this.speed = speed;
            this.timeRespawn = timeRespawn;
            this.timeInvulnerability = timeInvulnerability;
            this.type = type;
            this.strength = strength;
            this.prefabPath = prefabPath;
        }

        [JsonProperty] private readonly string name;
        [JsonProperty] private readonly float speed;
        [JsonProperty] private readonly int timeRespawn;
        [JsonProperty] private readonly int timeInvulnerability;
        [JsonProperty] private readonly float strength;
        [JsonProperty] private readonly Types type;
        [JsonProperty] private string prefabPath;

        public string Name
        {
            get { return name; }
        }

        public float Strength
        {
            get { return strength; }
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

        public string PrefabPath
        {
            get { return prefabPath; }
        }

        public Types Type
        {
            get { return type; }
        }

        public enum Types
        {
            Sample
        }
    }
}
