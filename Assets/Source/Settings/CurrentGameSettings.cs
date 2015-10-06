using System.Collections.Generic;
using System.IO;
using System.Linq;
using Caveman.Specification;
using UnityEngine;

namespace Caveman.Setting
{
    public class CurrentGameSettings
    {
        public List<BonusSpecification> TypeBonuses { get; private set; }
        public List<WeaponSpecification> TypeWeapons { get; private set; }
        public List<PlayerSpecification> TypePlayer { get; private set; }

        public readonly Dictionary<string, PlayerSpecification> DictionaryPlayer = new Dictionary<string, PlayerSpecification>();
        public readonly Dictionary<string, BonusSpecification> DictionaryBonuses = new Dictionary<string, BonusSpecification>();
        public readonly Dictionary<string, WeaponSpecification> DictionaryWeapons = new Dictionary<string, WeaponSpecification>();

        public static CurrentGameSettings Load(string folder)
        {
            var typeBonuses =
              SettingsHandler.ParseSettingsFromFile<List<BonusSpecification>>(Path.Combine(folder, "bonuses.json")).ToList();
            //typeBonuses.Sort();

            var typePlayers =
                SettingsHandler.ParseSettingsFromFile<List<PlayerSpecification>>(Path.Combine(folder, "players.json")).ToList();
            //typePlayers.Sort();

            var typeWeapons =
                SettingsHandler.ParseSettingsFromFile<List<WeaponSpecification>>(Path.Combine(folder, "weapons.json")).ToList();
            //typeWeapons.Sort();

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
