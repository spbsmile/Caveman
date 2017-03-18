using UnityEngine;

public static class FloatTools
{
    private const float DefaultTolerance = 0.2f;        // 0.02f

    public static float Normalize(this float currentValue, float min, float max)
    {
        if(currentValue > max)
            return max;
        if(currentValue < min)
            return min;
        return currentValue;
    }

    public static float LimitAngle(this float angle, float min, float max)
    {
        // if angle in the critic region...
        if (angle < 90 || angle > 270)
        {           
            if (angle>180) angle -= 360;  // convert all angles to -180..+180
            if (max>180) max -= 360;
            if (min>180) min -= 360;
        }

        angle = Mathf.Clamp(angle, min, max);
        if (angle < 0) angle += 360;  // if angle negative, convert to 0..360

        return angle;
    }

    public static bool IsApproximately(this float a, float b) 
    {
        return IsApproximately(a, b, DefaultTolerance);
    }
 
    public static bool IsApproximately(this float a, float b, float tolerance)
    {
        return Mathf.Abs(a - b) < tolerance;
    }

    public static bool IsAproximatelyOrMore(this float a, float b)
    {
        return IsAproximatelyOrMore(a, b, DefaultTolerance);
    }

    public static bool IsAproximatelyOrMore(this float a, float b, float tolerance)
    {
        return (IsApproximately(a, b, tolerance) || a > b);
    }

    public static bool IsAproximatelyOrLess(this float a, float b)
    {
        return IsAproximatelyOrLess(a, b, DefaultTolerance);
    }

    public static bool IsAproximatelyOrLess(this float a, float b, float tolerance)
    {
        return (IsApproximately(a, b, tolerance) || a < b);
    }

    public static float FixNaN(this float source)
    {
        if(float.IsNaN(source) || float.IsInfinity(source))
            return 0;
        return source;
    }

    public static float Modulo(float a, float b)
    {
        return a - b * Mathf.Floor(a / b);
    }
}
