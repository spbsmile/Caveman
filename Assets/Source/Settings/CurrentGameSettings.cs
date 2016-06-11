using System.Collections.Generic;
using Caveman.Configs;

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

        private CurrentGameSettings(IEnumerable<BonusConfig> bonusesConfigs, IEnumerable<PlayerConfig> playersConfigs, IEnumerable<WeaponConfig> weaponsConfigs, IEnumerable<ImageConfig> imageConfigs, IEnumerable<PoolsConfig> poolsConfig)
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

            foreach (var config in imageConfigs)
            {
                ImagesConfigs.Add(config.Name, config);
            }

            foreach (var config in poolsConfig)
            {
                PoolsConfigs.Add(config.Name, config);
            }
        }

        public static
            CurrentGameSettings Load(string bonuses, string weapons, string players, string pools,
                string images)
        {
            return new CurrentGameSettings(
                SettingsHandler.ParseSettingsFromFile<List<BonusConfig>>(bonuses),
                SettingsHandler.ParseSettingsFromFile<List<PlayerConfig>>(players),
                SettingsHandler.ParseSettingsFromFile<List<WeaponConfig>>(weapons),
                SettingsHandler.ParseSettingsFromFile<List<ImageConfig>>(images),
                SettingsHandler.ParseSettingsFromFile<List<PoolsConfig>>(pools)
                );
        }
    }
}