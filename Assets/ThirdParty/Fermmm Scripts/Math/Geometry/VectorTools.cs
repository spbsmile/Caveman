using UnityEngine;

public static class VectorTools
{

    /// <summary>
    /// Apply a rotation and scale to a Vector2 or Vector3. The same that the Transform component does but applied to Vectors.
    /// </summary>
    /// <param name="pointToTransform">Also works with Vector2</param>
    /// <param name="pivot">Also works with Vector2</param>
    /// <param name="rotation">Rotate around the pivot in degrees. To rotate in 2D use only the Z value and set 0 to X and Y axis</param>
    /// <param name="scale">Scale the point position.</param>
    /// <returns></returns>
    public static Vector3 TransformPoint(this Vector3 pointToTransform, Vector3 pivot, Vector3 rotation, Vector3 scale)
    {
        Matrix4x4 matrix = Matrix4x4.TRS(pivot, Quaternion.Euler(rotation), scale);
        Vector3 rotatedPoint = matrix.MultiplyPoint(pointToTransform - pivot);
        return rotatedPoint;
    }

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
        if(!Mathf.Approximately(vec1.x, vec2.x))
            return false;

        if(!Mathf.Approximately(vec1.y, vec2.y))
            return false;

        return true;
    }

    public static bool IsAproximately(this Vector3 vec1, Vector3 vec2)
    {
        return (Mathf.Approximately(vec1.x, vec2.x) && Mathf.Approximately(vec1.y, vec2.y) && Mathf.Approximately(vec1.z, vec2.z));
    }

    public static Vector2 FixNaN(this Vector2 source)
    {
        if(float.IsNaN(source.x) || float.IsInfinity(source.x))
            source.x = 0;
        if(float.IsNaN(source.y) || float.IsInfinity(source.y))
            source.y = 0;
        return source;
    }
}