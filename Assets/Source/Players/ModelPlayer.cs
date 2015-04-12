using UnityEngine;

namespace Caveman.Players
{
    public class ModelPlayer : ModelBasePlayer
    {
        private const float Speed = 1.2f;
        private const float ThresholdPosition = 0.1f;
        private Vector2 delta;
        private Animator animator;

        private Vector2 target;

        public void Start()
        {
            transform.position = Vector2.zero;
            animator = GetComponent<Animator>();
        }

        public void Update()
        {
            if (Input.GetMouseButton(0))
            {
                target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                delta = UnityExtensions.CalculateDelta(transform.position, target, Speed);
                animator.SetFloat("Speed", Speed);
            }
            if (Vector2.SqrMagnitude((Vector2)transform.position - target) < ThresholdPosition && delta.magnitude > ThresholdPosition)
            {
                delta = Vector2.zero;
                animator.SetFloat("Speed", 0);
                ThrowStone();
            }
            if (delta.magnitude > ThresholdPosition)
            {
                transform.position = new Vector3(transform.position.x + delta.x * Time.deltaTime,
                transform.position.y + delta.y * Time.deltaTime);    
            }
        }

        
    }
}
