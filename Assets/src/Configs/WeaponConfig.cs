using Caveman.Weapons;
using Newtonsoft.Json;

namespace Caveman.Configs
{
    [JsonObject(MemberSerialization.OptIn)]
    public class WeaponConfig : IConfig
    {
        public WeaponConfig(string name, float speed, float cooldown, int countItems, int weight,
            int rotateParameter, WeaponType type, string prefabPath)
        {
            this.name = name;
            this.speed = speed;
            this.cooldown = cooldown;
            this.countItems = countItems;
            this.weight = weight;
            this.rotateParameter = rotateParameter;
            this.type = type;
            this.prefabPath = prefabPath;
        }

        [JsonProperty] private readonly string name;
        [JsonProperty] private readonly float speed;
        [JsonProperty] private readonly float cooldown;
        [JsonProperty] private readonly int countItems;
        [JsonProperty] private readonly int weight;
        [JsonProperty] private readonly int rotateParameter;
        [JsonProperty] private readonly string prefabPath;
        [JsonProperty] private readonly WeaponType type;
        
        public string Name => name;

        public float Speed => speed;

        public float Cooldown => cooldown;

        public int Weight => weight;

        public int RotateParameter => rotateParameter;

        public int CountItems => countItems;

        public WeaponType Type => type;

        public string PrefabPath => prefabPath;
    }
}
