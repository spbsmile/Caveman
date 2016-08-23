using System.Collections.Generic;
using Caveman.Configs;
using Caveman.Configs.Levels;

namespace Caveman.Setting
{
    public class CurrentGameSettings
    {
        //  for get value propertes, key - isetting name
        public readonly Dictionary<string, PlayerConfig> PlayersConfigs = new Dictionary<string, PlayerConfig>();
        public readonly Dictionary<string, BonusConfig> BonusesConfigs = new Dictionary<string, BonusConfig>();
        public readonly Dictionary<string, WeaponConfig> WeaponsConfigs = new Dictionary<string, WeaponConfig>();
        public readonly Dictionary<string, ImageConfig> ImagesConfigs = new Dictionary<string, ImageConfig>();
        public readonly Dictionary<string, PoolsConfig> PoolsConfigs = new Dictionary<string, PoolsConfig>();
      /*  public readonly Dictionary<string, LevelSingleConfig> LevelsSingleConfigs = new Dictionary<string, LevelSingleConfig>(); */
        public readonly Dictionary<string, LevelMultiplayerConfig> LevelsMultiplayerConfigs = new Dictionary<string, LevelMultiplayerConfig>();

        private CurrentGameSettings(IEnumerable<BonusConfig> bonusesConfigs, IEnumerable<PlayerConfig> playersConfigs, IEnumerable<WeaponConfig> weaponsConfigs,
          IEnumerable<ImageConfig> imagesConfigs,
          IEnumerable<PoolsConfig> poolsConfig)//, IEnumerable<LevelSingleConfig> levelsSingleConfigs, IEnumerable<LevelMultiplayerConfig> levelsMultiplayerConfigs)
        {
            foreach (var config in bonusesConfigs)
            {
                BonusesConfigs.Add(config.Name, config);
            }

            foreach (var config in playersConfigs)
            {
                PlayersConfigs.Add(config.Name, config);
            }

            foreach (var config in weaponsConfigs)
            {
                WeaponsConfigs.Add(config.Name, config);
            }

            foreach (var config in imagesConfigs)
            {
                ImagesConfigs.Add(config.Name, config);
            }

            foreach (var config in poolsConfig)
            {
                PoolsConfigs.Add(config.Name, config);
            }
            /*
            foreach (var config in levelsSingleConfigs)
            {
                LevelsSingleConfigs.Add(config.Name, config);
            }

            foreach (var config in levelsMultiplayerConfigs)
            {
                LevelsMultiplayerConfigs.Add(config.Name, config);
            }*/
        }

        public static
            CurrentGameSettings Load(string bonuses, string weapons, string players, string pools,
                string images, string levelsSingle, string levelsMultiplayer)
        {
            return new CurrentGameSettings(
                SettingsHandler.ParseSettingsFromFile<List<BonusConfig>>(bonuses),
                SettingsHandler.ParseSettingsFromFile<List<PlayerConfig>>(players),
                SettingsHandler.ParseSettingsFromFile<List<WeaponConfig>>(weapons),
                SettingsHandler.ParseSettingsFromFile<List<ImageConfig>>(images),
                SettingsHandler.ParseSettingsFromFile<List<PoolsConfig>>(pools)/*,
                SettingsHandler.ParseSettingsFromFile<List<LevelSingleConfig>>(levelsSingle),
                SettingsHandler.ParseSettingsFromFile<List<LevelMultiplayerConfig>>(levelsMultiplayer)
                */);
        }
    }
}