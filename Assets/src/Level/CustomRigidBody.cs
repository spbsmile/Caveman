using UnityEngine;

namespace Caveman.Level
{
    // todo rename blockerMove
    public class CustomRigidBody : MonoBehaviour
    {
        public float Height;

        public float Width;

        public Vector2 Center;

        public Vector2 RightTop;

        public Vector2 LeftTop;

        public Vector2 LeftBottom;

        public Vector2 RightBottom;

        private SpriteRenderer spriteRenderer;

        public void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();

            var halfSizeX = spriteRenderer.bounds.size.x / 2;
            var halfSizeY = spriteRenderer.bounds.size.y / 2;

            LeftBottom = (Vector2)transform.position - new Vector2(halfSizeX, halfSizeY);
            LeftTop = (Vector2)transform.position - new Vector2(halfSizeX, -halfSizeY);

            RightBottom = (Vector2)transform.position - new Vector2(-halfSizeX, halfSizeY);
            RightTop = (Vector2)transform.position + new Vector2(halfSizeX, halfSizeY);
        }


        private void OnDrawGizmos()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();

            var halfSizeX = spriteRenderer.bounds.size.x / 2;
            var halfSizeY = spriteRenderer.bounds.size.y / 2;

            LeftBottom = (Vector2)transform.position - new Vector2(halfSizeX, halfSizeY);
            LeftTop = (Vector2)transform.position - new Vector2(halfSizeX, -halfSizeY);

            RightBottom = (Vector2)transform.position - new Vector2(-halfSizeX, halfSizeY);
            RightTop = (Vector2)transform.position + new Vector2(halfSizeX, halfSizeY);

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, 0.3f);

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(LeftTop, 0.3f);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(LeftBottom, 0.3f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(RightBottom, 0.3f);


            Gizmos.color = Color.black;
            Gizmos.DrawSphere(RightTop, 0.3f);

        }
    }
}