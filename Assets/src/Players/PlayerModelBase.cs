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
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(WeaponActionController))]
    /*
    * Player concept consists of two types: PlayerCore and PlayerModelBase
    * Type PlayerModelBase - Behavior
    * Type PlayerCore - contains all parameters/stats.
    */
    public class PlayerModelBase : MonoBehaviour, IDamageable
    {
        protected Func<Vector2> GetRandomPosition;
        protected Func<string, PlayerCore> GetPlayerById;

        protected SpriteRenderer spriteRenderer;
        //todo one parameter
        protected IServerNotify serverNotify;
        protected bool multiplayer;
        protected LevelMode levelMode;
        protected PlayerAnimation playerAnimation;
        protected Vector2 moveUnit;
        protected IWeapon currentWeapon;

        protected WeaponActionController weaponActionController;
        public PlayerCore Core { private set; get; }
        public bool IsVisible => spriteRenderer.isVisible;

        private PlayerPool pool;
        private const int CountWeaponAfterDrop = 0;

        protected void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            weaponActionController = GetComponent<WeaponActionController>();
        }

        public void Initialization(PlayerCore core, IServerNotify serverNotify,
            Func<Vector3, string, Vector3?> findClosestPlayer, PlayerPool pool,
            Func<Vector2> getRandomPosition,
            Transform imageDeath, LevelMode levelMode, Func<string, PlayerCore> getPlayerById)
        {
            this.serverNotify = serverNotify;
            this.pool = pool;
            GetRandomPosition = getRandomPosition;
            GetPlayerById = getPlayerById;
            if (serverNotify != null) multiplayer = true;
            Core = core;
            playerAnimation = new PlayerAnimation(GetComponent<Animator>(), imageDeath);
            this.levelMode = levelMode;
            weaponActionController.Initilization(findClosestPlayer, ActivateWeapon, core);
        }

        public virtual void PickupBonus(BonusBase bonus)
        {
            bonus.Effect(Core);
        }

        public virtual void Die()
        {
            Core.IsAlive = false;
            pool.Store(this);
        }

        // todo pickup controller
        // todo change weapon 
        public virtual void PickupWeapon(IWeapon weapon)
        {
            if (currentWeapon == null || currentWeapon.Config.Type != weapon.Config.Type)
            {
                weaponActionController.UpdateWeapon(weapon);
                currentWeapon = weapon;
                Core.WeaponCount = CountWeaponAfterDrop;
                Core.WeaponTypeChange?.Invoke(currentWeapon.Config.Type);
            }
            playerAnimation.Pickup();
            if (weapon.Config.Type == WeaponType.Sword)
            {
                weapon.Transform.parent = transform;
                weapon.Transform.localPosition = Vector2.zero;
            }
            weapon.Take(Core.Id);
        }

        public virtual void ActivateWeapon(Vector2 aim)
        {
            playerAnimation.Throw();
            currentWeapon.Activate(Core.Id, transform.position, aim);
        }

        protected virtual IEnumerator Respawn(Vector2 position)
        {
            yield return new WaitForSeconds(Core.Config.RespawnDuration);
            RespawnInstantly(position);
            StopMove();
            Core.IsAlive = true;
        }

        public virtual void RespawnInstantly(Vector2 position)
        {
            pool.New(Core.Id).transform.position = position;
            Core.Invulnerability = true;
            StartCoroutine(playerAnimation.Invulnerability(Core.Config.InvulnerabilityDuration, spriteRenderer));
            Core.Invulnerability = false;
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
            moveUnit = UnityExtensions.CalculateDelta(transform.position, targetPosition, Core.Speed);
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


