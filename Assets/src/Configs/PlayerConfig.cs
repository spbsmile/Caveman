using Caveman.Players;
using Newtonsoft.Json;

namespace Caveman.Configs
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PlayerConfig : IConfig
    {
        public PlayerConfig(string name, float speed, int respawnDuration, int invulnerabilityDuration, float strength,
            PlayerType type, string prefabPath)
        {
            this.name = name;
            this.speed = speed;
            this.respawnDuration = respawnDuration;
            this.invulnerabilityDuration = invulnerabilityDuration;
            this.type = type;
            this.strength = strength;
            this.prefabPath = prefabPath;
        }

        [JsonProperty] private readonly string name;
        [JsonProperty] private readonly float speed;
        [JsonProperty] private readonly int respawnDuration;
        [JsonProperty] private readonly int invulnerabilityDuration;
        [JsonProperty] private readonly float strength;
        [JsonProperty] private readonly PlayerType type;
        [JsonProperty] private readonly string prefabPath;

        public string Name => name;

        public float Strength => strength;

        public float Speed => speed;

        public int RespawnDuration => respawnDuration;

        public int InvulnerabilityDuration => invulnerabilityDuration;

        public string PrefabPath => prefabPath;

        public PlayerType Type => type;
    }
}
