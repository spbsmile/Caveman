using UnityEngine;

namespace Assets.Source.Weapons
{
    public class BezierTest : MonoBehaviour
    {
        private float k = 0.57f;
        private const int Deg = 30;
       
        public Vector2 p2;
        public Debug debug = Debug.Off;

        private Vector2 p0;
        private Vector2 p1;
        private float a;
        private float b;
        private float c;

        public void Start()
        {
            p0 = transform.position;

            a = k/(p0.x - p2.x);
            b = -a*(p0.x + p2.x);


            //a = (k - ((p2.y - p0.y)/(p2.x - p0.x)))/(p0.x - p2.x);
            //b = (p2.y - p0.y)/(p2.x - p0.x) - a*(p0.x + p2.x);
            c = (p2.x*p0.y - p0.x*p2.y)/(p2.x - p0.x) + a*p0.x*p2.x; 
            p1 = new Vector2(-b / (2 * a), ParabolaValue(a, b, c, -b / (2 * a)));
        }

        public void Update()
        {
            print(p2);

            if (debug == Debug.Run)
            {
                

            }

            k = Mathf.Tan(Mathf.Deg2Rad * (Deg + Vector2.Angle(Vector2.right, p2 - p0)));
            print(k + " k");

            Vector2.Angle(Vector2.right, p2 - p0);

            a = k / (p0.x - p2.x);
            b = -a * (p0.x + p2.x);

            //a = (k - ((p2.y - p0.y) / (p2.x - p0.x))) / (p0.x - p2.x);
            //b = (p2.y - p0.y) / (p2.x - p0.x) - a * (p0.x + p2.x);
            c = (p2.x * p0.y - p0.x * p2.y) / (p2.x - p0.x) + a * p0.x * p2.x;
            p1 = new Vector2(-b / (2 * a), ParabolaValue(a, b, c, -b / (2 * a)));

            print(Vector2.Angle(Vector2.right, p2 - p0));
            print(Vector2.Angle(p2 - p0, new Vector2(1, k)));
        }

        public  void Destroy()
        {
            Destroy(gameObject);
        }

        private float ParabolaValue(float a, float b, float c, float x)
        {
            return a*x*x + b*x + c;
        }

        public enum Debug
        {
            Run,
            Off
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawSphere(transform.position, 0.3f);
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(p1, 0.4f);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(p2, 0.5f);

            Gizmos.DrawRay(Vector2.zero, new Vector2(1,k));
            Gizmos.color = Color.red;
            Gizmos.DrawRay(p0, p2);
        }
    }
}
