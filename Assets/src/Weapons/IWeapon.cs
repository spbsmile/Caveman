using Caveman.Configs;
using Caveman.Players;

namespace Caveman.Weapons
{
    public interface IWeapon
    {
        void Destroy();
        void Damage();
        WeaponConfig Config {  get; }
        PlayerCore Owner { get; }
    }
}