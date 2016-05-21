using Caveman.Setting;
using System.Runtime.Serialization;

namespace Caveman.Specification
{
    [DataContract]
    public class WeaponSpecification : ISettings
    {
        //todo binding constr and data json
        public WeaponSpecification(string name, float speed, int timeRespawn, int countPickup, int throwInterval, int weight, int rotateParameter, Types type)
        {
            this.name = name;
            this.speed = speed;
            this.timeRespawn = timeRespawn;
            this.countPickup = countPickup;
            this.weight = weight;
            this.rotateParameter = rotateParameter;
            this.type = type;
        }

        [DataMember] private readonly string name;
        [DataMember] private readonly float speed;
        [DataMember] private readonly int timeRespawn;
        [DataMember] private readonly int countPickup;
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

        public int Weight
        {
            get { return weight; }
        }

        public int RotateParameter
        {
            get { return rotateParameter; }
        }

        public int CountPickup
        {
            get { return countPickup; }
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
