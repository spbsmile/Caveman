using Newtonsoft.Json;

namespace Caveman.Configs
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ImageConfig : IConfig
    {
        public ImageConfig(string name, string prefabPath)
        {
            this.name = name;
            this.prefabPath = prefabPath;
        }

        [JsonProperty] private readonly string name;
        [JsonProperty] private readonly string prefabPath;

        public string Name => name;
        public string PrefabPath => prefabPath;
    }
}
