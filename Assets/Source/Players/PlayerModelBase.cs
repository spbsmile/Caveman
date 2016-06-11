using System;
using System.Collections;
using System.Collections.Generic;
using Caveman.Bonuses;
using Caveman.CustomAnimation;
using Caveman.Network;
using Caveman.Pools;
using Caveman.Configs;
using Caveman.Utils;
using Caveman.Weapons;
using UnityEngine;
using Random = System.Random;

namespace Caveman.Players
{
    /*
     * Player concept consists of two types: Player and PlayerModelBase
     * Type PlayerModelBase - Behavior 
     * Type Player - contains all parameters/stats.
     */
    public class PlayerModelBase : MonoBehaviour
    {
        public Action<Vector2> Death;
        public Action RespawnGuiDisabled;
        // todo two some event. 
        public Func<WeaponConfig.Types, ObjectPool<WeaponModelBase>> ChangedWeaponsPool;
        protected Action ChangedWeapons;

        [HideInInspector] public BonusBase bonusBase;
        [HideInInspector] public SpriteRenderer spriteRenderer;
        [HideInInspector] public bool firstRespawn = true;
        [HideInInspector] public PlayerConfig Config;

        protected Vector2 target;
        protected Vector2 delta;
        protected Random r;
        protected bool lockedControl = true;

        //todo one parameter
        protected IClientListener serverNotify;
        protected bool multiplayer;
        
        protected WeaponConfig WeaponConfig;
        protected internal bool invulnerability;
        protected PlayerAnimation playerAnimation;
        protected List<PlayerModelBase> players;

        private PlayerPool poolPlayers;
        private ObjectPool<WeaponModelBase> poolWeapons;

        public float Speed { get; set; }
        public Player Player { private set; get; }

        protected virtual void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            playerAnimation = new PlayerAnimation(GetComponent<Animator>());
            Config = EnterPoint.CurrentSettings.PlayersConfigs["sample"];
            WeaponConfig = EnterPoint.CurrentSettings.WeaponsConfigs["stone"];
            Speed = Config.Speed;
        }

        public void Init(Player player, Random random, IClientListener serverNotify)
        {
            this.serverNotify = serverNotify;
            if (serverNotify != null) multiplayer = true;
            name = player.Name;
            transform.GetChild(0).GetComponent<TextMesh>().text = name;
            Player = player;
            players = new List<PlayerModelBase>();
            players.AddRange(poolPlayers.GetCurrentPlayers());
            poolPlayers.AddedPlayer += @base => players.Add(@base);
            poolPlayers.RemovePlayer += @base => players.Remove(@base);
            r = random;
        }

        public virtual void Play()
        {
            lockedControl = false;
        }

        public virtual void PickupBonus(BonusBase bonus)
        {
            bonus.Effect(this);
        }

        public virtual void Die()
        {
            Death(transform.position);
            poolPlayers.Store(this);
        }

        public virtual void PickupWeapon(WeaponModelBase weaponModel)
        {
            if (poolWeapons == null || weaponModel.Config.Type != WeaponConfig.Type)
            {
                poolWeapons = ChangedWeaponsPool(weaponModel.Config.Type);
                WeaponConfig = weaponModel.Config;
                if (ChangedWeapons != null)
                {
                    ChangedWeapons();    
                }
                else
                {
                    print("ChangedWeapons null" + name);
                }
            }
            playerAnimation.Pickup();
            weaponModel.Take();
        }

        public virtual void ThrowWeapon(Vector2 aim)
        {
            playerAnimation.Throw();
            poolWeapons.New().SetMotion(Player, transform.position, aim);
        }

        /// <summary>
        /// Gold spend player. Return true if gold enough.
        /// </summary>
        /// <param name="value">Amount spend gold</param>
        /// <returns>True if gold enough</returns>
        public virtual bool SpendGold(int value)
        {
            return true;
        }

        public virtual IEnumerator Respawn(Vector2 point)
        {
            yield return new WaitForSeconds(Config.TimeRespawn);
            Birth(point);
            StopMove();
            if (RespawnGuiDisabled != null) RespawnGuiDisabled();
        }

        public virtual void Birth(Vector2 point)
        {
            poolPlayers.New(Player.Id).transform.position = point;
            invulnerability = true;
            StartCoroutine(ProggressInvulnerability(Config.TimeInvulnerability));
        }

        // invoke, when respawn
        private IEnumerator ProggressInvulnerability(float playerTimeInvulnerability)
        {
            var startTime = Time.time;
            var render = spriteRenderer ? spriteRenderer : GetComponent<SpriteRenderer>();
            while (Time.time  < startTime + playerTimeInvulnerability)
            {
                render.enabled = false;
                yield return new WaitForSeconds(0.1f);
                render.enabled = true;
                yield return new WaitForSeconds(0.1f);
            }
            render.enabled = true;
            invulnerability = false;
        }

        /// <summary>
        /// linear motion
        /// </summary>
        protected virtual void Move()
        {
            if (lockedControl)
                return;
            transform.position = new Vector3(transform.position.x + delta.x * Time.deltaTime,
                transform.position.y + delta.y * Time.deltaTime);
            playerAnimation.SetMoving(delta.y < 0, delta.x > 0);
        }

        /// <summary>
        /// set delta - direction of motion
        /// </summary>
        /// <param name="target"></param>
        public void SetMove(Vector2 target)
        {
            this.target = target;
            if (lockedControl)
                return;
            delta = UnityExtensions.CalculateDelta(transform.position, target, Speed);
        }

        public void StopMove()
        {
            delta = Vector2.zero;
            playerAnimation.IsMoving_B = false;
            playerAnimation.IsMoving_F = false;
        }
    }
}


