using Caveman.Configs;
using UnityEngine;
using Random = System.Random;

namespace Caveman.Level
{
    public class MapCore
    {
        private readonly Random rand;

        private MapConfig Config { get; }
        public int Width { get; }
        public int Height { get; }

        public MapCore(int width, int height, MapConfig offlineConfig, bool isMultiplayer, MapModel model, Random rand,
            LevelMode levelMode)
        {
            this.rand = rand;
            MaxDistance = offlineConfig.Width * offlineConfig.Heght;
            Config = offlineConfig;
            Width = offlineConfig.Width;
            Height = offlineConfig.Heght;
            if (levelMode != LevelMode.designer)
            {
                model.CreateTerrain(rand, offlineConfig.PathPrefabTile, offlineConfig.Artefactses, offlineConfig.Width,
                    offlineConfig.Heght, isMultiplayer);
                if (!isMultiplayer) model.StartItemPeriodicals(offlineConfig.ItemsPeriodicals);
            }
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

        public Vector2 GetCenterMap()
        {
            return new Vector2(Width / 2, Height / 2);
        }

        /// <summary>
        /// roughly interval as max on map
        /// </summary>
        public float MaxDistance { get; }
    }
}