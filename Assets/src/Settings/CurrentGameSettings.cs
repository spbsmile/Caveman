using System.Collections.Generic;
using Caveman.Configs;
using Caveman.Configs.Levels;

namespace Caveman.Setting
{
    public class GameConfigs
    {
        // key - isetting name
        public readonly Dictionary<string, PlayerConfig> Player = new Dictionary<string, PlayerConfig>();
        public readonly Dictionary<string, BonusConfig> Bonus = new Dictionary<string, BonusConfig>();
        public readonly Dictionary<string, WeaponConfig> Weapon = new Dictionary<string, WeaponConfig>();
        public readonly Dictionary<string, ImageConfig> Image = new Dictionary<string, ImageConfig>();
        public readonly Dictionary<string, PoolsConfig> Pool = new Dictionary<string, PoolsConfig>();
        public readonly Dictionary<string, MapConfig> Map = new Dictionary<string, MapConfig>();
        public readonly Dictionary<string, SingleLevelConfig> SingleLevel = new Dictionary<string, SingleLevelConfig>(); 
        public readonly Dictionary<string, MultiplayerLevelConfig> MultiplayerLevel = new Dictionary<string, MultiplayerLevelConfig>();

        private GameConfigs(IEnumerable<BonusConfig> bonusesConfigs, IEnumerable<PlayerConfig> playersConfigs,
                IEnumerable<WeaponConfig> weaponsConfigs,
                IEnumerable<ImageConfig> imagesConfigs,
                IEnumerable<PoolsConfig> poolsConfig, IEnumerable<MapConfig> mapConfigs,
             IEnumerable<SingleLevelConfig> levelsSingleConfigs, IEnumerable<MultiplayerLevelConfig> levelsMultiplayerConfigs)
        {
            WriteConfig(bonusesConfigs, Bonus);
            WriteConfig(playersConfigs, Player);
            WriteConfig(weaponsConfigs, Weapon);
            WriteConfig(imagesConfigs, Image);
            WriteConfig(poolsConfig, Pool);
            WriteConfig(mapConfigs, Map);
            WriteConfig(levelsSingleConfigs, SingleLevel);
            WriteConfig(levelsMultiplayerConfigs, MultiplayerLevel);
        }

        public static
            GameConfigs Load(string bonuses, string weapons, string players, string pools,
                string images, string map, string levelsSingle, string levelsMultiplayer)
        {
            return new GameConfigs(
                SettingsHandler.ParseSettingsFromFile<List<BonusConfig>>(bonuses),
                SettingsHandler.ParseSettingsFromFile<List<PlayerConfig>>(players),
                SettingsHandler.ParseSettingsFromFile<List<WeaponConfig>>(weapons),
                SettingsHandler.ParseSettingsFromFile<List<ImageConfig>>(images),
                SettingsHandler.ParseSettingsFromFile<List<PoolsConfig>>(pools),
                SettingsHandler.ParseSettingsFromFile<List<MapConfig>>(map),
                SettingsHandler.ParseSettingsFromFile<List<SingleLevelConfig>>(levelsSingle),
                SettingsHandler.ParseSettingsFromFile<List<MultiplayerLevelConfig>>(levelsMultiplayer)
            );
        }

        private void WriteConfig<T>(IEnumerable<T> source, Dictionary<string, T> targetConfig) where T : IConfig
        {
            if (source == null) return;
            foreach (var config in source)
            {
                targetConfig.Add(config.Name, config);
            }
        }
    }
}