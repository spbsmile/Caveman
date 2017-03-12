using UnityEngine;

namespace Caveman.Level
{
    public class BlockerMove : MonoBehaviour
    {
        private Vector2 min;

        private Vector2 max;

        private SpriteRenderer render;

        public void Start()
        {
            render = GetComponent<SpriteRenderer>();
            min = render.bounds.min;
            max = render.bounds.max;
        }

        public Vector2 CorrectionMove(float x, float y, float halfHeroX)
        {
            if (min.y < y && y < max.y)
            {
                if (x > min.x - halfHeroX && x < max.x - 2 * halfHeroX)
                {
                    x = min.x - halfHeroX;
                }
                else if (x < max.x + halfHeroX && x > min.x + 2 * halfHeroX)
                {
                    x = max.x + halfHeroX;
                }
            }
            return new Vector2(x, y);
        }

        private void OnDrawGizmos()
        {
            render = GetComponent<SpriteRenderer>();

            min = render.bounds.min;

            max = render.bounds.max;

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, 0.3f);

            Gizmos.color = Color.green;

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(min, 0.3f);

            Gizmos.color = Color.yellow;

            Gizmos.color = Color.black;
            Gizmos.DrawSphere(max, 0.3f);
        }
    }
}