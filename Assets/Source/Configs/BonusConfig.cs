using System.Runtime.Serialization;
using Caveman.Setting;

namespace Caveman.Configs
{
    [DataContract]
    public class BonusConfig : ISettings
    {
        public BonusConfig(string name, float duration, Types type, string prefabPath)
        {
            this.name = name;
            this.duration = duration;
            this.type = type;
            this.prefabPath = prefabPath;
        }

        [DataMember] private readonly string name;
        [DataMember] private readonly int timeRespawn;
        [DataMember] private readonly float duration;
        [DataMember] private readonly Types type;
        [DataMember] private string prefabPath;

        public string Name
        {
            get { return name; }
        } 

        public int TimeRespawn
        {
            get { return timeRespawn; }
        }

        public float Duration
        {
            get { return duration; }
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
            Speed,
            Shield
        }
    }
}
