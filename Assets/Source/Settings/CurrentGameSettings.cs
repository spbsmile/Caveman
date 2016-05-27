using System.Collections.Generic;
using Caveman.Configs;

namespace Caveman.Setting
{
    public class CurrentGameSettings
    {
        /// <summary>
        /// 
        /// </summary>
        public List<BonusConfig> TypeBonuses { get; private set; }
        public List<WeaponConfig> TypeWeapons { get; private set; }
        public List<PlayerConfig> TypePlayer { get; private set; }

        /// <summary>
        ///  for get value propertes, key - isetting name
        /// </summary>
        public readonly Dictionary<string, PlayerConfig> DictionaryPlayer = new Dictionary<string, PlayerConfig>();
        public readonly Dictionary<string, BonusConfig> DictionaryBonuses = new Dictionary<string, BonusConfig>();
        public readonly Dictionary<string, WeaponConfig> DictionaryWeapons = new Dictionary<string, WeaponConfig>();

        public static CurrentGameSettings Load(string bonuses, string weapons, string players, string pools)
        {
            var bonusConfigs =
                SettingsHandler.ParseSettingsFromFile<List<BonusConfig>>(bonuses);

            var playerConfigs =
                SettingsHandler.ParseSettingsFromFile<List<PlayerConfig>>(players);

            var weaponConfigs =
                SettingsHandler.ParseSettingsFromFile<List<WeaponConfig>>(weapons);

            var poolsConfig = SettingsHandler.ParseSettingsFromFile<List<PoolsConfig>>(pools);

            return Create(bonusConfigs, playerConfigs, weaponConfigs);
        }

        private CurrentGameSettings(IEnumerable<BonusConfig> typeBonuses, IEnumerable<PlayerConfig> typePlayers,
            IEnumerable<WeaponConfig> typeWeapons)
        {
            foreach (var typeBonus in typeBonuses)
            {
                DictionaryBonuses.Add(typeBonus.Name, typeBonus);
            }

            foreach (var typePlayer in typePlayers)
            {
                DictionaryPlayer.Add(typePlayer.Name, typePlayer);
            }

            foreach (var typeWeapon in typeWeapons)
            {
                DictionaryWeapons.Add(typeWeapon.Name, typeWeapon);
            }
        }

        private static CurrentGameSettings Create(List<BonusConfig> typeBonuses, List<PlayerConfig> typePlayers,
            List<WeaponConfig> typeWeapons)
        {
            return new CurrentGameSettings(typeBonuses, typePlayers, typeWeapons)
            {
                TypeBonuses = typeBonuses,
                TypePlayer = typePlayers,
                TypeWeapons = typeWeapons
            };
        }
    }
}
