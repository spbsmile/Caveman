using System.Collections.Generic;
using Caveman.Configs;
using Caveman.Pools;

namespace Caveman.Setting
{
    public class CurrentGameSettings
    {
        /// <summary>
        /// for iterate config types
        /// </summary>
        public List<BonusConfig> TypeBonuses { get; private set; }
        public List<WeaponConfig> TypeWeapons { get; private set; }
        public List<PlayerConfig> TypePlayers { get; private set; }
        public List<ImageConfig>  TypeImages { get; private set; }

        /// <summary>
        ///  for get value propertes, key - isetting name
        /// </summary>
        public readonly Dictionary<string, PlayerConfig> DictionaryPlayer = new Dictionary<string, PlayerConfig>();
        public readonly Dictionary<string, BonusConfig> DictionaryBonuses = new Dictionary<string, BonusConfig>();
        public readonly Dictionary<string, WeaponConfig> DictionaryWeapons = new Dictionary<string, WeaponConfig>();
        public readonly Dictionary<string, ImageConfig> DictionaryImages = new Dictionary<string, ImageConfig>();

        public static CurrentGameSettings Load(string bonuses, string weapons, string players, string pools, string images)
        {
            var bonusConfigs =
                SettingsHandler.ParseSettingsFromFile<List<BonusConfig>>(bonuses);

            var playerConfigs =
                SettingsHandler.ParseSettingsFromFile<List<PlayerConfig>>(players);

            var weaponConfigs =
                SettingsHandler.ParseSettingsFromFile<List<WeaponConfig>>(weapons);

            var poolsConfig = SettingsHandler.ParseSettingsFromFile<List<PoolsConfig>>(pools);

            var imagesConfig = SettingsHandler.ParseSettingsFromFile<List<ImageConfig>>(images);

            return Create(bonusConfigs, playerConfigs, weaponConfigs, imagesConfig);
        }

        private CurrentGameSettings(IEnumerable<BonusConfig> bonusesConfigs, IEnumerable<PlayerConfig> playersConfigs, IEnumerable<WeaponConfig> weaponsConfigs, List<ImageConfig> typeImages)
        {
            foreach (var bonusConfig in bonusesConfigs)
            {
                DictionaryBonuses.Add(bonusConfig.Name, bonusConfig);
            }

            foreach (var playerConfig in playersConfigs)
            {
                DictionaryPlayer.Add(playerConfig.Name, playerConfig);
            }

            foreach (var weaponConfig in weaponsConfigs)
            {
                DictionaryWeapons.Add(weaponConfig.Name, weaponConfig);
            }
        }

        private static CurrentGameSettings Create(List<BonusConfig> typeBonuses, List<PlayerConfig> typePlayers, List<WeaponConfig> typeWeapons, List<ImageConfig> typeImages)
        {
            return new CurrentGameSettings(typeBonuses, typePlayers, typeWeapons, typeImages)
            {
                TypeBonuses = typeBonuses,
                TypePlayers = typePlayers,
                TypeWeapons = typeWeapons,
                TypeImages = typeImages
            };
        }
    }
}
