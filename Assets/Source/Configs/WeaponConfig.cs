using Caveman.Setting;
using System.Runtime.Serialization;

namespace Caveman.Configs
{
    [DataContract]
    public class WeaponConfig : ISettings
    {
        public WeaponConfig(string name, float speed, int timeRespawn, float cooldown, int countItems, int weight, int rotateParameter, Types type)
        {
            this.name = name;
            this.speed = speed;
            this.timeRespawn = timeRespawn;
            this.cooldown = cooldown;
            this.countItems = countItems;
            this.weight = weight;
            this.rotateParameter = rotateParameter;
            this.type = type;
        }

        [DataMember] private readonly string name;
        [DataMember] private readonly float speed;
        [DataMember] private readonly int timeRespawn;
        [DataMember] private readonly float cooldown;
        [DataMember] private readonly int countItems;
        [DataMember] private readonly int weight;
        [DataMember] private readonly int rotateParameter;
        [DataMember] private readonly Types type;

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

        public enum Types
        {
            Stone,
            Skull
        }
    }
}
