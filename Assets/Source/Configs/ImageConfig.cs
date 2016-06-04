using System.Runtime.Serialization;
using Caveman.Setting;

namespace Caveman.Configs
{
    [DataContract]
    public class ImageConfig : ISettings
    {
        public ImageConfig(string name, string prefabPath)
        {
            this.name = name;
            this.prefabPath = prefabPath;
        }

        [DataMember]
        private string name;
        [DataMember]
        private string prefabPath;

        public string Name { get { return name; } }
        public string PrefabPath {  get { return prefabPath; } }
    }
}
