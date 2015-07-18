using System.Collections;
using Caveman.Setting;
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

        public static Vector2 ConvectorCoordinate(Vector2 point)
        {
            var x = (point.x / Multiplayer.HeigthMapServer) * Settings.HeightMap;
            var y = (point.y / Multiplayer.WidthMapServer) * Settings.WidthMap;
            return new Vector2(x, y);
        }

        public static string GenerateKey(Vector2 point)
        {
            return point.x + ":" + point.y;
        }
    }
}
