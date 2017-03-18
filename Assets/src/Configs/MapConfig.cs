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

        public string Name
        {
            get { return name; }
        }

        public int Width
        {
            get { return width; }
        }

        public int Heght
        {
            get { return height; }
        }

        public string PathPrefabTile
        {
            get { return pathPrefabTile; }
        }

        public IEnumerable<Artefacts> Artefactses
        {
            get { return artefacts; }
        }

        public List<ItemPeriodical> ItemsPeriodicals
        {
            get { return itemsPeriodical; }
        }

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

            public string Name { get { return name; } }
            public string PathPrefab { get { return pathPrefab; } }
            public int Count { get { return count; } }
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

            public string Name { get { return name; } }
            public int Period { get { return period; } }
            public int Count { get { return count; } }
            public string Type { get { return type; } }
        }
    }
}
