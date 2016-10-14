using Caveman.Configs;
using UnityEngine;
using Random = System.Random;

namespace Caveman.Level
{
    public class MapCore
    {
        private readonly Random rand;

        public MapConfig Config { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public MapCore(MapConfig config, bool isMultiplayer, MapModel model, Random rand)
        {
            this.rand = rand;
            MaxDistance = config.Width*config.Heght;
            Config = config;
            Width = config.Width;
            Height = config.Heght;
            model.CreateTerrain(rand, config.PathPrefabTile, config.Artefactses, config.Width, config.Heght, isMultiplayer);
            if (!isMultiplayer) model.StartItemPeriodicals(config.ItemsPeriodicals);
        }

        public Vector2 RandomPosition
        {
            get { return new Vector2(rand.Next(Width), rand.Next(Height)); }
        }

        // for Func delegate  
        public Vector2 GetRandomPosition()
        {
            return new Vector2(rand.Next(Width), rand.Next(Height));
        }

        /// <summary>
        /// roughly interval as max on map
        /// </summary>
        public float MaxDistance { get; private set; }
    }
}