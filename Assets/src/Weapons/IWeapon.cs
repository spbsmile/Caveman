using Caveman.Configs;
using UnityEngine;

namespace Caveman.Weapons
{
    public interface IWeapon
    {
        string Id { get; }
        WeaponConfig Config { get; }
        string OwnerId { get; }
        void Activate(string ownerId, Vector2 from, Vector2 to);
        void Take();
        void Destroy();
    }
}