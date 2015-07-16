using System;
using Caveman.Bonuses;
using Caveman.Network;
using Caveman.Setting;
using Caveman.Utils;
using Caveman.Weapons;
using UnityEngine;
using Random = System.Random;

namespace Caveman.Players
{
    public class PlayerModelBase : MonoBehaviour
    {
        public Action<Vector2> Death;
        public Action<Player> Respawn;
        public Func<WeaponType, ObjectPool> ChangedWeapons;

        public Player player;
        public BonusBase bonusType;

        protected Vector2 delta;
        protected Animator animator;
        protected Vector2 target;
        protected Random r;
        protected ServerConnection serverConnection;
        protected bool multiplayer;

        protected SpriteRenderer renderer;
        private bool inMotion;
        protected PlayerPool playersPool;
        private ObjectPool weaponsPool;
        private WeaponType weaponType;
        protected PlayerModelBase[] players;

        public float Speed { get; set; }

        protected virtual void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void Init(Player player, Vector2 start, Random random, PlayerPool pool, ServerConnection serverConnection)
        {
            this.serverConnection = serverConnection;
            if (serverConnection != null) multiplayer = true;
            name = player.name;
            transform.GetChild(0).GetComponent<TextMesh>().text = name;
            this.player = player;
            playersPool = pool;
            // todo при сервере подписки на добавление удаление игроков
            players = pool.Players;
            r = random;
            transform.position = start;
            Speed = Settings.SpeedPlayer;
            renderer = GetComponent<SpriteRenderer>();
        }

        protected void PickupBonus(BonusBase bonus)
        {
            if (multiplayer) serverConnection.SendPickBonus(transform.position, (int)bonus.Type);
            bonus.Effect(this);
        }

        protected void PickupWeapon(WeaponModelBase weaponModel)
        {
            if (player.Weapons > Settings.MaxCountWeapons) return;
            print("PickupWeapon");
            if (multiplayer) serverConnection.SendPickWeapon(transform.position, (int)weaponModel.type);
            if (weaponsPool == null || weaponModel.type != weaponType)
            {
                player.Weapons = 0;
                weaponsPool = ChangedWeapons(weaponModel.type);
                weaponType = weaponModel.type;
            }
            player.Weapons += 1;
            animator.SetTrigger(Settings.AnimPickup);
            weaponModel.Take();
        }

        protected void Throw(Vector2 aim)
        {
            if (multiplayer) serverConnection.SendUseWeapon(aim, (int)weaponType);
            weaponsPool.New().GetComponent<WeaponModelBase>().SetMotion(player, transform.position, aim);
            player.Weapons--;
        }

        //todo переписать
        public bool InMotion
        {
            protected get
            {
                if (Vector2.SqrMagnitude(delta) > UnityExtensions.ThresholdPosition &&
                    Vector2.SqrMagnitude((Vector2)transform.position - target) < UnityExtensions.ThresholdPosition)
                {
                    animator.SetFloat(delta.y > 0 ? Settings.AnimRunB : Settings.AnimRunF, 0);
                    delta = Vector2.zero;
                    inMotion = false;
                    return inMotion;
                }
                return inMotion;
            }
            set { inMotion = value; }
        }

        protected void Move()
        {
            if (Vector2.SqrMagnitude(delta) > UnityExtensions.ThresholdPosition)
            {
                var position = new Vector3(transform.position.x + delta.x * Time.deltaTime,
               transform.position.y + delta.y * Time.deltaTime);
                transform.position = position;
            }
        }
    }
}


