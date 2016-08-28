using System.Collections.Generic;
using Caveman.Setting;
using Newtonsoft.Json;

namespace Caveman.Configs
{
    [JsonObject(MemberSerialization.OptIn)]
    public class MapConfig : ISettings
    {
        [JsonProperty] private readonly string name;
        [JsonProperty] private readonly int width;
        [JsonProperty] private readonly int height;
        [JsonProperty] private readonly string pathPrefabTile;
        [JsonProperty] private readonly List<Artefacts> artefacts;

        public MapConfig(string name, int width, int height, string pathPrefabTile, List<Artefacts> artefacts)
        {
            this.name = name;
            this.height = height;
            this.width = width;
            this.pathPrefabTile = pathPrefabTile;
            this.artefacts = artefacts;
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

        public List<Artefacts> Artefactses
        {
            get { return artefacts; }
        }

        [JsonObject(MemberSerialization.OptIn)]
        public class Artefacts : ISettings
        {
            [JsonProperty] private string name;
            [JsonProperty] private string pathPrefab;
            [JsonProperty] private int count;

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
    }
}
