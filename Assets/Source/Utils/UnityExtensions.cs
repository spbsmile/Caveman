using System.Collections;
using UnityEngine;

namespace Caveman.Utils
{
    public static class UnityExtensions
    {
        public const float ThresholdPosition = 0.1f;

        public static Vector2 CalculateDelta(Vector2 currentPosition, Vector2 targetPosition, float speed)
        {
            var dx = targetPosition.x - currentPosition.x;
            var dy = targetPosition.y - currentPosition.y;
            var d = Mathf.Sqrt(dx*dx + dy*dy);
            return d != 0 ? new Vector2((dx/d)*speed, (dy/d)*speed) : Vector2.zero;
        }

        public static IEnumerator FadeOut(SpriteRenderer spriteRenderer)
        {
            for (var i = 1f; i > 0; i -= 0.1f)
            {
                var c = spriteRenderer.color;
                c.a = i;
                spriteRenderer.color = c;
                yield return null;
            }
        }

        public static IEnumerator FadeIn(SpriteRenderer spriteRenderer)
        {
            var color = spriteRenderer.color;
            color.a = 0;
            spriteRenderer.color = color;
            for (var i = 0f; i < 1; i += 0.1f)
            {
                if (spriteRenderer)
                {
                    var c = spriteRenderer.color;
                    c.a = i;
                    spriteRenderer.color = c;
                }
                yield return null;
            }
        }

        public static IEnumerator FadeOut(this CanvasGroup canvasGroup)
        {
            canvasGroup.alpha = 1;
            for (var i = 1f; i > 0; i -= 0.1f)
            {
                canvasGroup.alpha = i;
                yield return null;
            }
            canvasGroup.alpha = 0;
        }
    }
}
