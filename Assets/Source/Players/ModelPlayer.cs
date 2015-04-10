using UnityEngine;

namespace Caveman.Players
{
    public class ModelPlayer : ModelBasePlayer
    {
        private const int Speed = 2;
        private Vector2 positionDelta;

        public void Start()
        {
            transform.position = Vector2.zero;
        }

        public void Update()
        {
            if (Input.GetMouseButton(0))
            {
                positionDelta = CalculateDelta(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
            transform.position = new Vector3(transform.position.x + positionDelta.x*Time.deltaTime,
                transform.position.y + positionDelta.y*Time.deltaTime);

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
