using System;
using UnityEngine;

namespace Caveman.CustomAnimation
{
    public class PlayerAnimation
    {
        private Animator animator;
        private Transform transform;
        private Transform name;

        public PlayerAnimation(Animator animator)
        {
            this.animator = animator;
            transform = animator.GetComponent<Transform>();
            name = transform.GetChild(0);
        }

        public bool IsMoving_F
        {
            get { return animator.GetBool("IsMoving_F"); }
            set { animator.SetBool("IsMoving_F", value); }
        }

        public bool IsMoving_B
        {
            get { return animator.GetBool("IsMoving_B"); }
            set { animator.SetBool("IsMoving_B", value); }
        }

        public void Throw()
        {
            animator.SetTrigger(IsMoving_F ? "throw_f" : "throw_b");
        }

        public void Pickup()
        {
            // todo no has pickup_b in project
            animator.SetTrigger(IsMoving_F ? "pickup_f" : "pickup_f");
        }

        public void SetMoving(bool front, bool directionRight)
        {
            if (directionRight && Math.Abs(transform.eulerAngles.y - 180) > 0.1)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                name.localEulerAngles = new Vector3(0, 180, 0);
            }
            if (!directionRight && Math.Abs(transform.eulerAngles.y) > 0.1)
            {
                transform.eulerAngles = Vector3.zero;
                name.eulerAngles = Vector3.zero;
            }
            
            if (IsMoving_B && front)
            {
                IsMoving_B = false;
                IsMoving_F = true;
            }
            else if (IsMoving_F && !front)
            {
                IsMoving_B = true;
                IsMoving_F = false;
            }
            else if (!IsMoving_B && !IsMoving_F)
            {
                if (!front)
                {
                    IsMoving_B = true;
                }
                else
                {
                    IsMoving_F = true;
                }
            }
        }
    }
}