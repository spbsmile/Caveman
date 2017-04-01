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

        public string Name => name;

        public int WeaponsRare => weaponsRare;

        public int WeaponsOrdinary => weaponsOrdinary;

        public int WeaponsPopular => weaponsPopular;

        public int BonusesRare => bonusesRare;

        public int BonusesOrdinary => bonusesOrdinary;

        public int BonusesPopular => bonusesPopular;

        public int ImagesRare => imagesRare;

        public int ImagesOrdinary => imagesOrdinary;

        public int ImagesPopular => imagesPopular;
    }
}
