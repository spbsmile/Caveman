using System;
using Caveman.Configs;

namespace Caveman.Level
{
    public class MapCore
    {
        public MapConfig Config { private set; get; }

        private MapModel model;

        public MapCore(MapConfig config, bool isMultiplayer, MapModel model, Random rand)
        {
            Config = config;
            this.model = model;
            model.CreateTerrain(rand, config.PathPrefabTile, config.Artefactses, config.Width, config.Heght, isMultiplayer);
            model.StartItemPeriodicals(config.ItemsPeriodicals);
        }
    }
}