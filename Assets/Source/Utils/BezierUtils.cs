using System;
using UnityEngine;

namespace Caveman.Utils
{
    public class BezierUtils
    {
        public BezierUtils()
        {
        }

        /**
         * Calculates bezier position value according given start and and point, using time t
         */
        public static Vector2 Bezier2(Vector2 Start, Vector2 Control, Vector2 End , float t)
        {
            return (((1-t)*(1-t)) * Start) + (2 * t * (1 - t) * Control) + ((t * t) * End);
        }

        /**
         * Calculates control point for bezier function
         * @param offsetRate -- indicates rate of the control point offset distance,
         *                      if it is 1, then distance between points is used,
         *                      if 2, then half of distance between points, etc.
         */
        public static Vector2 ControlPoint(Vector2 startPoint, Vector2 endPoint, float offsetRate = 3)
        {
            Vector2 result;
            float distance = Vector2.Distance(startPoint, endPoint);
            Vector2 centerPoint = Vector2.Lerp(startPoint, endPoint, 0.5f);
            float r = distance / offsetRate;
            double angle = Math.Atan((endPoint.y - startPoint.y) / (endPoint.x - startPoint.x));

            float desiredX = r * (float)Math.Cos(angle + Math.PI / 2);
            float desiredY = r * (float)Math.Sin(angle + Math.PI / 2);

            desiredX += centerPoint.x;
            desiredY += centerPoint.y;

            return new Vector2(desiredX, desiredY);
        }
    }
}

