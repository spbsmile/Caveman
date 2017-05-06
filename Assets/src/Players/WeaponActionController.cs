using System;
using System.Collections;
using System.Collections.Generic;
using Caveman.Weapons;
using UnityEngine;

namespace Caveman.Players
{
    public class WeaponActionController : MonoBehaviour
    {
	    private Func<Vector3, string, Vector3?> FindClosestPlayerPosition;
		private Action<Vector2> Activate;
		private IWeapon currentWeapon;
		private PlayerCore core;

	    private bool isMeleeMode;

	    private float lastTimeActivateMeleeWeapon;
	    // config
	    private const float CooldownMeleeWeapon = 2f;

	    private const float RaycastRadius = 0.7f;

		public void Initilization(
			Func<Vector3, string, Vector3?> find, 
			Action<Vector2> activate,
			PlayerCore core)
		{
			FindClosestPlayerPosition = find;
			Activate = activate;
			this.core = core;
		}

		public void UpdateWeapon(IWeapon weapon)
        {
	        if (currentWeapon != weapon)
	        {
		        currentWeapon = weapon;
		        PerfomActionFSM();
	        }
        }

	    public void FixedUpdate()
	    {
		    if (isMeleeMode)
		    {
			    //https://docs.unity3d.com/ScriptReference/Physics2D.Raycast.html
			    print("hello fixed update melee weapon");
				var direction = -transform.right.normalized;
			    var hit = Physics2D.Raycast(transform.position, direction, RaycastRadius);

				// Physics2D.Raycast(rayCasterPosition + directionOfRayCast*
				// this.collider2D.bounds.size.magnitude, directionOfRayCast, distance+1);


			    // layermask
			    if (hit.collider != null && Time.timeSinceLevelLoad - lastTimeActivateMeleeWeapon > CooldownMeleeWeapon)
			    {
					print("hello hit");
				    print(hit.collider.name);
				    lastTimeActivateMeleeWeapon = Time.timeSinceLevelLoad;
				    // target position of melee weapon
				    Activate(transform.position);
				    print("hello collider from melee weapon");
			    }
		    }
	    }

	    private void PerfomActionFSM()
	    {
		    if (currentWeapon?.Config.Type == WeaponType.Skull || currentWeapon?.Config.Type == WeaponType.Stone)
		    {
			    isMeleeMode = false;
			    StartCoroutine(PerfomRangeWeapon());
		    }
		    else if (currentWeapon?.Config.Type == WeaponType.Sword)
		    {
			    StopCoroutine(PerfomRangeWeapon());
			    isMeleeMode = true;
		    }
	    }

		private IEnumerator PerfomRangeWeapon()
		{
			yield return new WaitForSeconds(currentWeapon?.Config.Cooldown ?? 2f);
			if (core.WeaponCount > 0)
			{
				var targetPosition = FindClosestPlayerPosition(transform.position, core.Id);
				if (targetPosition != null)
				{
					core.WeaponCount--;
					Activate(targetPosition.Value);
				}
			}
			StartCoroutine(PerfomRangeWeapon());
		}

	     private void OnDrawGizmos()
	     {
		     Gizmos.color = Color.cyan;
		     Gizmos.DrawSphere(transform.position, 0.1f);

			 Gizmos.color = Color.red;
			 Gizmos.DrawRay(transform.position, transform.right);


			 Gizmos.color = Color.blue;
			 Gizmos.DrawRay(transform.position, transform.forward);

			 Gizmos.color = Color.green;
			 Gizmos.DrawRay(transform.position, transform.up);
	     }
    }
}


