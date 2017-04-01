using System.Collections.Generic;
using Newtonsoft.Json;

namespace Caveman.Configs
{
    [JsonObject(MemberSerialization.OptIn)]
    public class MapConfig : IConfig
    {
        [JsonProperty] private readonly string name;
        [JsonProperty] private readonly int width;
        [JsonProperty] private readonly int height;
        [JsonProperty] private readonly string pathPrefabTile;
        [JsonProperty] private readonly List<Artefacts> artefacts;
        [JsonProperty] private readonly List<ItemPeriodical> itemsPeriodical;

        public MapConfig(string name, int width, int height, string pathPrefabTile, List<Artefacts> artefacts, List<ItemPeriodical> itemsPeriodical)
        {
            this.name = name;
            this.height = height;
            this.width = width;
            this.pathPrefabTile = pathPrefabTile;
            this.artefacts = artefacts;
            this.itemsPeriodical = itemsPeriodical;
        }

        public string Name => name;

        public int Width => width;

        public int Heght => height;

        public string PathPrefabTile => pathPrefabTile;

        public IEnumerable<Artefacts> Artefactses => artefacts;

        public List<ItemPeriodical> ItemsPeriodicals => itemsPeriodical;

        [JsonObject(MemberSerialization.OptIn)]
        public class Artefacts : IConfig
        {
            [JsonProperty] private readonly string name;
            [JsonProperty] private readonly string pathPrefab;
            [JsonProperty] private readonly int count;

            public Artefacts(string name, string pathPrefab, int count)
            {
                this.name = name;
                this.pathPrefab = pathPrefab;
                this.count = count;
            }

            public string Name => name;
            public string PathPrefab => pathPrefab;
            public int Count => count;
        }

        [JsonObject(MemberSerialization.OptIn)]
        public class ItemPeriodical: IConfig
        {
            [JsonProperty] private readonly string name;
            [JsonProperty] private readonly int period;
            [JsonProperty] private readonly int count;
            [JsonProperty] private readonly string type;

            public ItemPeriodical(string name, int period, int count, string type)
            {
                this.name = name;
                this.period = period;
                this.count = count;
                this.type = type;
            }

            public string Name => name;
            public int Period => period;
            public int Count => count;
            public string Type => type;
        }
    }
}
