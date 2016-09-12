using System;
using System.Collections;
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

        protected Vector2 moveUnit;
        protected Random r;

        //todo one parameter
        protected IServerNotify serverNotify;
        protected bool multiplayer;
        
        protected WeaponConfig WeaponConfig;
        protected internal bool invulnerability;
        protected PlayerAnimation playerAnimation;
	    protected PlayersManager playersManager;
	    private ObjectPool<WeaponModelBase> currentPoolWeapons;

        public PlayerCore PlayerCore { private set; get; }

	    private PlayerPool playerPool;

	    protected virtual void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            playerAnimation = new PlayerAnimation(GetComponent<Animator>());
            WeaponConfig = EnterPoint.CurrentSettings.WeaponsConfigs["stone"];
        }

        public void Init(PlayerCore playerCore, Random random, IServerNotify serverNotify, PlayersManager playersManager, PlayerPool playerPool)
        {
            this.serverNotify = serverNotify;
	        this.playersManager = playersManager;
	        this.playerPool = playerPool;
	        if (serverNotify != null) multiplayer = true;
            PlayerCore = playerCore;
            r = random;
        }
        
        public virtual void PickupBonus(BonusBase bonus)
        {
            bonus.Effect(this);
        }

        public virtual void Die()
        {
	        PlayerCore.IsAlive = false;
            playerPool.Store(this);
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

        public virtual void ActivateWeapon(Vector2 aim)
        {
            playerAnimation.Throw();
            currentPoolWeapons.New().SetMotion(PlayerCore, transform.position, aim);
        }

        public virtual IEnumerator Respawn(Vector2 position)
        {
            yield return new WaitForSeconds(PlayerCore.Config.RespawnDuration);
            RespawnInstantly(position);
            StopMove();
	        PlayerCore.IsAlive = true;
        }

        public virtual void RespawnInstantly(Vector2 position)
        {
            playerPool.New(PlayerCore.Id).transform.position = position;
            invulnerability = true;
            StartCoroutine(ProggressInvulnerability(PlayerCore.Config.InvulnerabilityDuration));
        }

        // invoke, when respawn
        private IEnumerator ProggressInvulnerability(float duration)
        {
            var startTime = Time.time;
            var render = spriteRenderer ? spriteRenderer : GetComponent<SpriteRenderer>();
            while (Time.time  < startTime + duration)
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
            moveUnit = UnityExtensions.CalculateDelta(transform.position, targetPosition, PlayerCore.Speed);
        }

        public void StopMove()
        {
            moveUnit = Vector2.zero;
            playerAnimation.IsMoving_B = false;
            playerAnimation.IsMoving_F = false;
        }
    }
}


