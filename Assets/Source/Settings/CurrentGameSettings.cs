using System.Collections.Generic;
using Caveman.Configs;
using Caveman.Configs.Levels;

namespace Caveman.Setting
{
    public class CurrentGameSettings
    {
        // key - isetting name
        public readonly Dictionary<string, PlayerConfig> PlayersConfigs = new Dictionary<string, PlayerConfig>();
        public readonly Dictionary<string, BonusConfig> BonusesConfigs = new Dictionary<string, BonusConfig>();
        public readonly Dictionary<string, WeaponConfig> WeaponsConfigs = new Dictionary<string, WeaponConfig>();
        public readonly Dictionary<string, ImageConfig> ImagesConfigs = new Dictionary<string, ImageConfig>();
        public readonly Dictionary<string, PoolsConfig> PoolsConfigs = new Dictionary<string, PoolsConfig>();
        public readonly Dictionary<string, MapConfig> MapConfigs = new Dictionary<string, MapConfig>();
      /*  public readonly Dictionary<string, LevelSingleConfig> LevelsSingleConfigs = new Dictionary<string, LevelSingleConfig>(); */
        public readonly Dictionary<string, LevelMultiplayerConfig> LevelsMultiplayerConfigs = new Dictionary<string, LevelMultiplayerConfig>();

        private CurrentGameSettings(IEnumerable<BonusConfig> bonusesConfigs, IEnumerable<PlayerConfig> playersConfigs,
                IEnumerable<WeaponConfig> weaponsConfigs,
                IEnumerable<ImageConfig> imagesConfigs,
                IEnumerable<PoolsConfig> poolsConfig, IEnumerable<MapConfig> mapConfigs)
            //, IEnumerable<LevelSingleConfig> levelsSingleConfigs, IEnumerable<LevelMultiplayerConfig> levelsMultiplayerConfigs)
        {
            WriteConfig(bonusesConfigs, BonusesConfigs);
            WriteConfig(playersConfigs, PlayersConfigs);
            WriteConfig(weaponsConfigs, WeaponsConfigs);
            WriteConfig(imagesConfigs, ImagesConfigs);
            WriteConfig(poolsConfig, PoolsConfigs);
            WriteConfig(mapConfigs, MapConfigs);
            //WriteConfig(levelsSingleConfigs, LevelsSingleConfigs);
            //WriteConfig(levelsMultiplayerConfigs, LevelsMultiplayerConfigs);
        }

        public static
            CurrentGameSettings Load(string bonuses, string weapons, string players, string pools,
                string images, string map, string levelsSingle, string levelsMultiplayer)
        {
            return new CurrentGameSettings(
                SettingsHandler.ParseSettingsFromFile<List<BonusConfig>>(bonuses),
                SettingsHandler.ParseSettingsFromFile<List<PlayerConfig>>(players),
                SettingsHandler.ParseSettingsFromFile<List<WeaponConfig>>(weapons),
                SettingsHandler.ParseSettingsFromFile<List<ImageConfig>>(images),
                SettingsHandler.ParseSettingsFromFile<List<PoolsConfig>>(pools), 
                SettingsHandler.ParseSettingsFromFile<List<MapConfig>>(map)/*,
                SettingsHandler.ParseSettingsFromFile<List<LevelSingleConfig>>(levelsSingle),
                SettingsHandler.ParseSettingsFromFile<List<LevelMultiplayerConfig>>(levelsMultiplayer)
                */);
        }

        private void WriteConfig<T>(IEnumerable<T> source, Dictionary<string, T> targetConfig) where T : ISettings
        {
            foreach (var config in source)
            {
                targetConfig.Add(config.Name, config);
            }
        }
    }
}