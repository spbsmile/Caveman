using System.Runtime.Serialization;
using Caveman.Setting;

namespace Caveman.Configs
{
    [DataContract]
    public class PoolsConfig : ISettings
    {
        public PoolsConfig(string name, int weaponsRare, int weaponsOrdinary, int weaponsPopular, int deathImages)
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
        [DataMember] private int bonusesRare;
        [DataMember] private int bonusesOrdinary;
        [DataMember] private int bonusesPopular;

        public string Name { get { return name; } }

        public int WeaponsRare { get { return weaponsRare; } }

        public int WeaponsOrdinary { get { return weaponsOrdinary; } }

        public int WeaponsPopular { get { return weaponsPopular; } }

        public int BonusesRare { get { return bonusesRare; } }

        public int BonusesOrdinary { get { return bonusesOrdinary; } }

        public int BonusesPopular { get { return bonusesPopular; } }

        public int DeathImages { get { return deathImages; } }
    }
}
