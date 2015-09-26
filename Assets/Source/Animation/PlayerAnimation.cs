using UnityEngine;

namespace Caveman.CustomAnimation
{
    public class PlayerAnimation
    {
        private Animator animator;

        public PlayerAnimation(Animator animator)
        {
            this.animator = animator;
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

        public void SetMoving(bool front)
        {
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