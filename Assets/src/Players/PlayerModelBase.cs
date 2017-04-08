using System;
using System.Collections;
using Caveman.Bonuses;
using Caveman.CustomAnimation;
using Caveman.Network;
using Caveman.Pools;
using Caveman.Level;
using Caveman.Utils;
using Caveman.Weapons;
using UnityEngine;


namespace Caveman.Players
{
    /*
     * Player concept consists of two types: PlayerCore and PlayerModelBase
     * Type PlayerModelBase - Behavior 
     * Type PlayerCore - contains all parameters/stats.
     */
    public class PlayerModelBase : MonoBehaviour, IDamageable
    {
        protected Func<Vector2> GetRandomPosition;
        protected Func<PlayerModelBase, PlayerModelBase> FindClosestPlayer;
        protected Func<string, PlayerCore> GetPlayerById;

        [HideInInspector] public BonusBase bonusBase;
        [HideInInspector] public SpriteRenderer spriteRenderer;

        //todo one parameter
        protected IServerNotify serverNotify;
        protected bool multiplayer;
        protected LevelMode levelMode;
        
        protected PlayerAnimation playerAnimation;
        protected Vector2 moveUnit;
        protected IWeapon currentWeapon;

        public PlayerCore PlayerCore { private set; get; }

	    private PlayerPool pool;

        protected void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Initialization(PlayerCore core, IServerNotify serverNotify,
            Func<PlayerModelBase, PlayerModelBase> findClosestPlayer, PlayerPool pool,
            Func<Vector2> getRandomPosition,
            Transform imageDeath, LevelMode levelMode, Func<string, PlayerCore> getPlayerById)
        {
            this.serverNotify = serverNotify;
	        this.pool = pool;
            GetRandomPosition = getRandomPosition;
            FindClosestPlayer = findClosestPlayer;
            GetPlayerById = getPlayerById;
            if (serverNotify != null) multiplayer = true;
            PlayerCore = core;
            playerAnimation = new PlayerAnimation(GetComponent<Animator>(), imageDeath);
            this.levelMode = levelMode;
        }
        
        public virtual void PickupBonus(BonusBase bonus)
        {
            bonus.Effect(PlayerCore);
        }

        public virtual void Die()
        {
            PlayerCore.IsAlive = false;
            pool.Store(this);
        }

        public virtual void PickupWeapon(IWeapon weapon)
        {
            if (currentWeapon == null || currentWeapon.Config.Type != weapon.Config.Type)
            {
                currentWeapon = weapon;
                PlayerCore.WeaponCount = 0;
            }
            playerAnimation.Pickup();
            weapon.Take();
        }

        public virtual void ActivateWeapon(Vector2 aim)
        {
            playerAnimation.Throw();
            currentWeapon.Activate(PlayerCore.Id, transform.position, aim);
        }

        protected virtual IEnumerator Respawn(Vector2 position)
        {
            yield return new WaitForSeconds(PlayerCore.Config.RespawnDuration);
            RespawnInstantly(position);
            StopMove();
	        PlayerCore.IsAlive = true;
        }

        public virtual void RespawnInstantly(Vector2 position)
        {
            pool.New(PlayerCore.Id).transform.position = position;
            PlayerCore.Invulnerability = true;
            StartCoroutine(playerAnimation.Invulnerability(PlayerCore.Config.InvulnerabilityDuration, spriteRenderer));
            PlayerCore.Invulnerability = false;
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

        public virtual void CalculateMoveUnit(Vector2 targetPosition)
        {
            moveUnit = UnityExtensions.CalculateDelta(transform.position, targetPosition, PlayerCore.Speed);
        }

        protected void StopMove()
        {
            moveUnit = Vector2.zero;
            playerAnimation.IsMoving_B = false;
            playerAnimation.IsMoving_F = false;
        }

        public void ApplyDamage(float mount)
        {
            throw new NotImplementedException();
        }
    }
}


