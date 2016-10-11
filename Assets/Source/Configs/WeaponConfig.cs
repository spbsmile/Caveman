using Caveman.Setting;
using Newtonsoft.Json;

namespace Caveman.Configs
{
    [JsonObject(MemberSerialization.OptIn)]
    public class WeaponConfig : ISettings
    {
        public WeaponConfig(string name, float speed, int timeRespawn, float cooldown, int countItems, int weight,
            int rotateParameter, Types type, string prefabPath)
        {
            this.name = name;
            this.speed = speed;
            this.timeRespawn = timeRespawn;
            this.cooldown = cooldown;
            this.countItems = countItems;
            this.weight = weight;
            this.rotateParameter = rotateParameter;
            this.type = type;
            this.prefabPath = prefabPath;
        }

        [JsonProperty] private readonly string name;
        [JsonProperty] private readonly float speed;
        [JsonProperty] private readonly int timeRespawn;
        [JsonProperty] private readonly float cooldown;
        [JsonProperty] private readonly int countItems;
        [JsonProperty] private readonly int weight;
        [JsonProperty] private readonly int rotateParameter;
        [JsonProperty] private readonly string prefabPath;
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

        public float Cooldown
        {
            get { return cooldown; }
        }

        public int Weight
        {
            get { return weight; }
        }

        public int RotateParameter
        {
            get { return rotateParameter; }
        }

        public int CountItems
        {
            get { return countItems; }
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
            Stone,
            Skull
        }
    }
}
