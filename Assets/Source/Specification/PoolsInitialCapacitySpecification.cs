using System.Runtime.Serialization;
using Caveman.Setting;

namespace Caveman.Specification
{
    [DataContract]
    public class PoolsInitialCapacitySpecification : ISettings
    {
        public PoolsInitialCapacitySpecification(string name, int weaponsRare, int weaponsOrdinary, int weaponsPopular, int deathImages)
        {
            this.name = name;
            this.weaponsRare = weaponsRare;
            this.weaponsOrdinary = weaponsOrdinary;
            this.weaponsPopular = weaponsPopular;
            this.deathImages = deathImages;
        }

        [DataMember] private readonly string name;
        [DataMember] private readonly int weaponsRare;
        [DataMember] private readonly int weaponsOrdinary;
        [DataMember] private readonly int weaponsPopular;
        [DataMember] private readonly int deathImages;

        public string Name { get { return name; } }

        public int WeaponsRare { get { return weaponsRare; } }

        public int WeaponsOrdinary { get { return weaponsOrdinary; } }

        public int WeaponsPopular { get { return weaponsPopular; } }

        public int DeathImages { get { return deathImages; } }
    }
}
