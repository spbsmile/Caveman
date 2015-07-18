using Caveman.Setting;
using Caveman.Utils;
using UnityEngine;

namespace Caveman.Animation
{
    public class StoneSplash : MonoBehaviour
    {
        public float speed;

        private Vector2 delta;
        private Vector2 target;

        private bool afterInit;
        private ObjectPool<StoneSplash> pool;

        public void Update()
        {
            if (Vector2.SqrMagnitude(delta) > UnityExtensions.ThresholdPosition)
            {
                if (Vector2.SqrMagnitude(target - (Vector2)transform.position) > UnityExtensions.ThresholdPosition)
                {
                    transform.position = new Vector2(transform.position.x + delta.x*Time.deltaTime,
                        transform.position.y + delta.y*Time.deltaTime);
                    transform.Rotate(Vector3.forward, Settings.RotateStoneParameter);
                }
                else
                {
                    Destroy();
                }
            }
            else
            {
                if (afterInit)
                {
                    Destroy();
                }
            }
        }

        public void Init(int i, Vector2 position, ObjectPool<StoneSplash> pool)
        {
            this.pool = pool;
            transform.position = position;
            if (i == 0)
            {
                target = position + (0.5f)*Vector2.right;
            }
            if (i == 1)
            {
                target = position - (0.5f)*Vector2.right;
            }
            if (i == 2)
            {
                target = position + (0.5f)*Vector2.up;
            }
            if (i == 3)
            {
                target = position - (0.5f)*Vector2.up;
            }
            afterInit = true;
            delta = UnityExtensions.CalculateDelta(transform.position, target, speed);
        }

        private void Destroy()
        {
            delta = Vector2.zero;
            pool.Store(this);
        }
    }
}
