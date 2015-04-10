using UnityEngine;

namespace Caveman.Players
{
    public class ModelPlayer : ModelBasePlayer
    {
        private const float Speed = 2;
        private Vector2 delta;
        private Animator animator;

        public void Start()
        {
            transform.position = Vector2.zero;
            animator = GetComponent<Animator>();
        }

        public void Update()
        {
            if (Input.GetMouseButton(0))
            {
                delta = CalculateDelta(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                animator.SetFloat("Speed", Speed);
            }
            transform.position = new Vector3(transform.position.x + delta.x*Time.deltaTime,
                transform.position.y + delta.y*Time.deltaTime);

        }

        private Vector2 CalculateDelta(Vector2 positionTarget)
        {
            var dx = positionTarget.x - transform.position.x;
            var dy = positionTarget.y - transform.position.y;
            var d = Mathf.Sqrt(dx*dx + dy*dy);
            return d != 0 ? new Vector2((dx/d)*Speed, (dy/d)*Speed) : Vector2.zero;
        }
    }
}
