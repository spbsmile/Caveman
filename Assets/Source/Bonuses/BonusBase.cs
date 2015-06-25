using UnityEngine;
using Caveman.Utils;
using Caveman.Players;
using Caveman.Setting;
using System;

namespace Caveman.Bonuses
{
	public class BaseBonus : MonoBehaviour
	{
		protected Animator animator;
		
		private ObjectPool pool;
		private string name;
		private float duration;
		private System.Random r; 
		
		public void Start()
		{
			animator = GetComponent<Animator>();
		}
		
		public void Init(System.Random random, ObjectPool pool, string name)
		{
			this.name = name;
			this.pool = pool;
			r = random;
			
		}

		public void RandomPosition()
		{
			transform.position = new Vector2(r.Next(-Settings.Br, Settings.Br), r.Next(-Settings.Br, Settings.Br));
		}
		
		public void OnTriggerEnter2D(Collider2D other)
		{
			if(other.gameObject.GetComponent<BasePlayerModel>())
			{
				
			}	            
		}  
		
		public virtual void Effect(BasePlayerModel playerModel)
		{
			
			playerModel.Speed = playerModel.Speed*2;
			/// no func
			playerModel.ResetBonus -= playerModel.Speed = playerModel.Speed/2;
		}
	}
}


