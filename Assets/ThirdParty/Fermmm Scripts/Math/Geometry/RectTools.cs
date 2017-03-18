using UnityEngine;
using System;


public static class RectTools
{
    public static float Distance(Rect a, Rect b)
    {
        if (a.x < b.xMax && b.x < a.xMax)
        {
            if (a.y < b.yMax && b.y < a.yMax)
                return 0;
            else if (a.y > b.y)
                return a.y - b.yMax;
            else
                return b.y - a.yMax;
        }
        else if (a.y < b.yMax && b.y < a.yMax)
        {
            if (a.x > b.x)
                return a.x - b.xMax;
            else
                return b.x - a.xMax;
        }
        else if (a.x > b.x)
        {
            if (a.y > b.y)
                return Distance(a.x, a.y, b.xMax, b.yMax);

            return Distance(a.x, a.yMax, b.xMax, b.y);
        }
        else if (a.y > b.y)
            return Distance(a.xMax, a.y, b.x, b.yMax);
        else
            return Distance(a.xMax, a.yMax, b.x, b.y);
    }

    public static float Distance(float x1, float y1, float x2, float y2)
    {
        return (float)Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
    }

    public static Rect CombineWith(this Rect rect1, Rect rect2)
    {
        Rect result = new Rect();

        if(rect1.x < rect2.x)
            result.x = rect1.x;
        else
            result.x = rect2.x;

        if(rect1.y < rect2.y)
            result.y = rect1.y;
        else
            result.y = rect2.y;

        if(rect1.x + rect1.width > rect2.x + rect2.width)
            result.width = rect1.x + rect1.width - result.x;
        else
            result.width = rect2.x + rect2.width - result.x;

        if(rect1.y + rect1.height > rect2.y + rect2.height)
            result.height = rect1.y + rect1.height - result.y;
        else
            result.height = rect2.y + rect2.height - result.y;

        return result;
    }
}
