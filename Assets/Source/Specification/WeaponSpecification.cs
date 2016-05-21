using Caveman.Setting;
using System.Runtime.Serialization;

namespace Caveman.Specification
{
    [DataContract]
    public class WeaponSpecification : ISettings
    {
        //todo binding constr and data json
        public WeaponSpecification(string name, float speed, int timeRespawn, int countPickup, int throwInterval, int maxOnPlayer, int initialLying, int rotateParameter, Types type)
        {
            this.name = name;
            this.speed = speed;
            this.timeRespawn = timeRespawn;
            this.countPickup = countPickup;
            this.throwInterval = throwInterval;
            this.maxOnPlayer = maxOnPlayer;
            this.initialLying = initialLying;
            this.rotateParameter = rotateParameter;
            this.type = type;
        }

        [DataMember] private readonly string name;
        [DataMember] private readonly float speed;
        [DataMember] private readonly int timeRespawn;
        [DataMember] private readonly int countPickup;
        [DataMember] private readonly int throwInterval;
        [DataMember] private readonly int maxOnPlayer;
        [DataMember] private readonly int initialLying;
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

        //todo bad logic. need move on player specification. Diff player specification - dif value of parameter
        public int ThrowInterval
        {
            get { return throwInterval; }
        }
        
        //todo bad logic. need move on player specification. Diff player specification - dif value of parameter
        public int MaxOnPLayer
        {
            get { return maxOnPlayer; }
        }

        public int InitialLying
        {
            get { return initialLying; }
        }

        public int RotateParameter
        {
            get { return rotateParameter; }
        }

        //todo bad logic. need move on player specification. Diff player specification - dif value of parameter
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
