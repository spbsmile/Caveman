using System;
using System.Collections;
using System.Collections.Generic;
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
        public Func<WeaponType, ObjectPool<WeaponModelBase>> ChangedWeaponsPool;

        public Player player;
        public BonusBase bonusType;
        public string Id;
        public float Speed { get; set; }
        public SpriteRenderer spriteRenderer;

        protected Action ChangedWeapons;
        protected Vector2 delta;
        protected Animator animator;
        protected Vector2 target;
        protected Random r;
        protected ServerConnection serverConnection;
        protected bool multiplayer;
        protected WeaponType weaponType;
        //todo переделать под массив
        protected List<PlayerModelBase> players;

        private PlayerPool poolPlayers;
        private ObjectPool<WeaponModelBase> poolWeapons;

        protected virtual void Start()
        {
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            Speed = Settings.SpeedPlayer;
        }

        public virtual void Init(Player player, Vector2 start, Random random, PlayerPool pool, ServerConnection serverConnection)
        {
            this.serverConnection = serverConnection;
            if (serverConnection != null) multiplayer = true;
            name = player.name;
            transform.GetChild(0).GetComponent<TextMesh>().text = name;
            this.player = player;
            poolPlayers = pool;
            players = new List<PlayerModelBase>();
            players.AddRange(poolPlayers.GetCurrentPlayers());
            poolPlayers.AddedPlayer += @base => players.Add(@base);
            r = random;
            transform.position = start;
        }

        public virtual void PickupBonus(BonusBase bonus)
        {
            bonus.Effect(this);
        }

        public virtual void Die()
        {
            poolPlayers.Store(this);
            Death(transform.position);
        }

        public virtual void PickupWeapon(WeaponModelBase weaponModel)
        {
            if (poolWeapons == null || weaponModel.type != weaponType)
            {
                poolWeapons = ChangedWeaponsPool(weaponModel.type);
                weaponType = weaponModel.type;
                ChangedWeapons();
            }
            animator.SetTrigger(Settings.AnimPickup);
            weaponModel.Take();
        }

        public virtual void Throw(Vector2 aim)
        {
            poolWeapons.New().SetMotion(player, transform.position, aim);
        }

        public virtual IEnumerator Respawn()
        {
            yield return new WaitForSeconds(Settings.TimeRespawnPlayer);
            poolPlayers.New(Id).transform.position = new Vector2(r.Next(Settings.WidthMap), r.Next(Settings.HeightMap));
        }

        public virtual void StandStill()
        {
            animator.SetFloat(delta.y > 0 ? Settings.AnimRunB : Settings.AnimRunF, 0);
            delta = Vector2.zero;
        }

        protected void Move()
        {
            transform.position = new Vector3(transform.position.x + delta.x*Time.deltaTime,
                transform.position.y + delta.y*Time.deltaTime);
        }

        public void SetPool(PlayerPool pool)
        {
            poolPlayers = pool;
        }
    }
}


