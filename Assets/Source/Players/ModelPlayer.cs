using UnityEngine;

namespace Caveman.Players
{
    public class ModelPlayer : ModelBasePlayer
    {
        private const float Speed = 1.2f;
        private Vector2 delta;
        private Vector2 target;

        public void Update()
        {
            if (Input.GetMouseButton(0))
            {
                target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                delta = UnityExtensions.CalculateDelta(transform.position, target, Speed);
                animator.SetFloat("Speed", Speed);
            }
            if (delta.magnitude > UnityExtensions.ThresholdPosition && Vector2.SqrMagnitude((Vector2)transform.position - target) < UnityExtensions.ThresholdPosition)
            {
                delta = Vector2.zero;
                animator.SetFloat("Speed", 0);
                ThrowStone();
            }
            if (delta.magnitude > UnityExtensions.ThresholdPosition)
            {
                transform.position = new Vector3(transform.position.x + delta.x * Time.deltaTime,
                transform.position.y + delta.y * Time.deltaTime);    
            }
        }
    }
}
