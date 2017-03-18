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

        [JsonProperty] private string name;
        [JsonProperty] private string prefabPath;

        public string Name { get { return name; } }
        public string PrefabPath {  get { return prefabPath; } }
    }
}
