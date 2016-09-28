using UnityEngine;

public static class FloatTools
{
    public static float Normalize(float currentValue, float min, float max)
    {
        if(currentValue > max)
            return max;
        if(currentValue < min)
            return min;
        return currentValue;
    }

    public static bool IsApproximately(this float a, float b) 
    {
        return IsApproximately(a, b, 0.02f);
    }
 
    public static bool IsApproximately(this float a, float b, float tolerance)
    {
        return Mathf.Abs(a - b) < tolerance;
    }

    public static bool IsAproximatelyOrMore(this float a, float b)
    {
        return IsAproximatelyOrMore(a, b, 0.02f);
    }

    public static bool IsAproximatelyOrMore(this float a, float b, float tolerance)
    {
        return (IsApproximately(a, b, tolerance) || a > b);
    }

    public static bool IsAproximatelyOrLess(this float a, float b)
    {
        return IsAproximatelyOrLess(a, b, 0.02f);
    }

    public static bool IsAproximatelyOrLess(this float a, float b, float tolerance)
    {
        return (IsApproximately(a, b, tolerance) || a < b);
    }

    public static float FixNaN(this float source)
    {
        if(float.IsNaN(source))
            return 0;
        return source;
    }
}
