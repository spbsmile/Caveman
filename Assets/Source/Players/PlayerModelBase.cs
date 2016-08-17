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
     * Player concept consists of two types: PlayerCore and PlayerModelBase
     * Type PlayerModelBase - Behavior 
     * Type PlayerCore - contains all parameters/stats.
     */
    public class PlayerModelBase : MonoBehaviour
    {
        public Func<WeaponConfig.Types, ObjectPool<WeaponModelBase>> ChangedWeaponsPool;        

        [HideInInspector] public BonusBase bonusBase;
        [HideInInspector] public SpriteRenderer spriteRenderer;
        [HideInInspector] public PlayerConfig Config;

        protected Vector2 moveUnit;
        protected Random r;
       // protected bool lockedControl = true;

        //todo one parameter
        protected IClientListener serverNotify;
        protected bool multiplayer;
        
        protected WeaponConfig WeaponConfig;
        protected internal bool invulnerability;
        protected PlayerAnimation playerAnimation;
        protected List<PlayerModelBase> players;

        private PlayerPool poolPlayers;
        private ObjectPool<WeaponModelBase> currentPoolWeapons;

        public float Speed { get; set; }
        public PlayerCore PlayerCore { private set; get; }

        protected virtual void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            playerAnimation = new PlayerAnimation(GetComponent<Animator>());
            Config = EnterPoint.CurrentSettings.PlayersConfigs["sample"];
            WeaponConfig = EnterPoint.CurrentSettings.WeaponsConfigs["stone"];
            Speed = Config.Speed;
        }

        public void Init(PlayerCore playerCore, Random random, IClientListener serverNotify)
        {
            //todo replace to playersmanager
            this.serverNotify = serverNotify;
            if (serverNotify != null) multiplayer = true;
            PlayerCore = playerCore;
            // todo replace to playercore
            players = new List<PlayerModelBase>();
            players.AddRange(poolPlayers.GetCurrentPlayers());
            poolPlayers.AddedPlayer += @base => players.Add(@base);
            poolPlayers.RemovePlayer += @base => players.Remove(@base);
            r = random;
        }
        /*
        public virtual void Play()
        {
            lockedControl = false;
        }*/
        
        public virtual void PickupBonus(BonusBase bonus)
        {
            bonus.Effect(this);
        }

        public virtual void Die()
        {
	        PlayerCore.IsAlive = false;
            poolPlayers.Store(this);
        }

        public virtual void PickupWeapon(WeaponModelBase weaponModel)
        {
            if (currentPoolWeapons == null || weaponModel.Config.Type != WeaponConfig.Type)
            {
                currentPoolWeapons = ChangedWeaponsPool(weaponModel.Config.Type);
                WeaponConfig = weaponModel.Config;
                PlayerCore.WeaponCount = 0;
            }
            playerAnimation.Pickup();
            weaponModel.Take();
        }

        public virtual void ThrowWeapon(Vector2 aim)
        {
            playerAnimation.Throw();
            currentPoolWeapons.New().SetMotion(PlayerCore, transform.position, aim);
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
	        PlayerCore.IsAlive = true;
        }

        public virtual void Birth(Vector2 point)
        {
            poolPlayers.New(PlayerCore.Id).transform.position = point;
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
            transform.position = new Vector3(transform.position.x + moveUnit.x * Time.deltaTime,
                transform.position.y + moveUnit.y * Time.deltaTime);
            playerAnimation.SetMoving(moveUnit.y < 0, moveUnit.x > 0);
        }

        public void CalculateMoveUnit(Vector2 targetPosition)
        {
            moveUnit = UnityExtensions.CalculateDelta(transform.position, targetPosition, Speed);
        }

        public void StopMove()
        {
            moveUnit = Vector2.zero;
            playerAnimation.IsMoving_B = false;
            playerAnimation.IsMoving_F = false;
        }
    }
}


