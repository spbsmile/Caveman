using System.Runtime.Serialization;
using Caveman.Setting;

namespace Caveman.Configs
{
    [DataContract]
    public class MapConfig : ISettings
    {
        public MapConfig(string name, int width, int height)
        {
            this.name = name;
            this.height = height;
            this.width = width;
        }

        [DataMember] private readonly string name;
        [DataMember] private readonly int width;
        [DataMember] private readonly int height;


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
