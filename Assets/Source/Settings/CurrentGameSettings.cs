using System.Collections.Generic;
using System.Linq;
using Caveman.Specification;

namespace Caveman.Setting
{
    public class CurrentGameSettings
    {
        /// <summary>
        /// 
        /// </summary>
        public List<BonusSpecification> TypeBonuses { get; private set; }
        public List<WeaponSpecification> TypeWeapons { get; private set; }
        public List<PlayerSpecification> TypePlayer { get; private set; }

        /// <summary>
        ///  for get value propertes, key - isetting name
        /// </summary>
        public readonly Dictionary<string, PlayerSpecification> DictionaryPlayer = new Dictionary<string, PlayerSpecification>();
        public readonly Dictionary<string, BonusSpecification> DictionaryBonuses = new Dictionary<string, BonusSpecification>();
        public readonly Dictionary<string, WeaponSpecification> DictionaryWeapons = new Dictionary<string, WeaponSpecification>();

        public static CurrentGameSettings Load()
        {
            var typeBonuses =
                SettingsHandler.ParseSettingsFromFile<List<BonusSpecification>>("bonuses");

            var typePlayers =
                SettingsHandler.ParseSettingsFromFile<List<PlayerSpecification>>("players");

            var typeWeapons =
                SettingsHandler.ParseSettingsFromFile<List<WeaponSpecification>>("weapons");

            return Create(typeBonuses, typePlayers, typeWeapons);
        }

        private CurrentGameSettings(IEnumerable<BonusSpecification> typeBonuses, IEnumerable<PlayerSpecification> typePlayers,
            IEnumerable<WeaponSpecification> typeWeapons)
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

        private static CurrentGameSettings Create(List<BonusSpecification> typeBonuses, List<PlayerSpecification> typePlayers,
            List<WeaponSpecification> typeWeapons)
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
