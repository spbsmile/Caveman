using UnityEngine;

public static class VectorTools
{
    public static Vector2 Abs(this Vector2 vec)
    {
        return new Vector2(Mathf.Abs(vec.x), Mathf.Abs(vec.y));
    }

    public static Vector3 Abs(this Vector3 vec)
    {
        return new Vector3(Mathf.Abs(vec.x), Mathf.Abs(vec.y), Mathf.Abs(vec.z));
    }

    public static bool IsAproximately(this Vector2 vec1, Vector2 vec2)
    {
        return (Mathf.Approximately(vec1.x, vec2.x) && Mathf.Approximately(vec1.y, vec2.y));
    }

    public static bool IsAproximately(this Vector3 vec1, Vector3 vec2)
    {
        return (Mathf.Approximately(vec1.x, vec2.x) && Mathf.Approximately(vec1.y, vec2.y) && Mathf.Approximately(vec1.z, vec2.z));
    }
}