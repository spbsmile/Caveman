using System;
using Caveman.Configs;

namespace Caveman.Level
{
    public class MapCore
    {
        public MapConfig Config { set; get; }

        public MapCore(MapConfig config, bool isMultiplayer, MapModel model, Random rand)
        {
            Config = config;
            model.CreateTerrain(rand, config.PathPrefabTile, config.Artefactses, config.Width, config.Heght, isMultiplayer);
            if (!isMultiplayer) model.StartItemPeriodicals(config.ItemsPeriodicals);
        }
    }
}