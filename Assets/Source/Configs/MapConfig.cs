using Caveman.Setting;
using Newtonsoft.Json;

namespace Caveman.Configs
{
    [JsonObject(MemberSerialization.OptIn)]
    public class MapConfig : ISettings
    {
        public MapConfig(string name, int width, int height)
        {
            this.name = name;
            this.height = height;
            this.width = width;
        }

        [JsonProperty] private readonly string name;
        [JsonProperty] private readonly int width;
        [JsonProperty] private readonly int height;


        public string Name
        {
            get { return name; }
        }

        public int Width
        {
            get { return width; }
        }

        public int Heght
        {
            get { return height; }
        }
    }
}
