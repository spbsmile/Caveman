using Caveman.Configs;
using UnityEngine;

namespace Caveman.Weapons
{
    public interface IWeapon
    {
        string Id { get; }
        WeaponConfig Config { get; }

        Transform Transform { get; }
        void Activate(string ownerId, Vector2 from, Vector2 to);
        void Take();
    }
}