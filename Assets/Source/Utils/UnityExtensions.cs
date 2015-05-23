using UnityEngine;

namespace Caveman.Utils
{
    public static class UnityExtensions
    {
        public static Vector2 CalculateDelta(Vector2 position, Vector2 target, float speed)
        {
            var dx = target.x - position.x;
            var dy = target.y - position.y;
            var d = Mathf.Sqrt(dx * dx + dy * dy);
            return d != 0 ? new Vector2((dx / d) * speed, (dy / d) * speed) : Vector2.zero;
        }

        public const float ThresholdPosition = 0.1f;
    }
}
