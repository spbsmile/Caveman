using Newtonsoft.Json;

namespace Caveman.Configs
{
    /// <summary>
    /// define initial count items in pools, may be deleted this file later
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class PoolsConfig : IConfig
    {
        public PoolsConfig(string name, int weaponsRare, int weaponsOrdinary, int weaponsPopular, int imagesRare,
            int imagesOrdinary, int imagesPopular)
        {
            this.name = name;
            this.weaponsRare = weaponsRare;
            this.weaponsOrdinary = weaponsOrdinary;
            this.weaponsPopular = weaponsPopular;
            this.imagesRare = imagesRare;
            this.imagesOrdinary = imagesOrdinary;
            this.imagesPopular = imagesPopular;
        }

        [JsonProperty] private readonly string name;
        [JsonProperty] private readonly int weaponsRare;
        [JsonProperty] private readonly int weaponsOrdinary;
        [JsonProperty] private readonly int weaponsPopular;
        [JsonProperty] private int bonusesRare;
        [JsonProperty] private int bonusesOrdinary;
        [JsonProperty] private int bonusesPopular;
        [JsonProperty] private int imagesRare;
        [JsonProperty] private int imagesOrdinary;
        [JsonProperty] private int imagesPopular;

        public string Name
        {
            get { return name; }
        }

        public int WeaponsRare
        {
            get { return weaponsRare; }
        }

        public int WeaponsOrdinary
        {
            get { return weaponsOrdinary; }
        }

        public int WeaponsPopular
        {
            get { return weaponsPopular; }
        }

        public int BonusesRare
        {
            get { return bonusesRare; }
        }

        public int BonusesOrdinary
        {
            get { return bonusesOrdinary; }
        }

        public int BonusesPopular
        {
            get { return bonusesPopular; }
        }

        public int ImagesRare
        {
            get { return imagesRare; }
        }

        public int ImagesOrdinary
        {
            get { return imagesOrdinary; }
        }

        public int ImagesPopular
        {
            get { return imagesPopular; }
        }
    }
}
