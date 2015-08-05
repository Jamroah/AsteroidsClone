using UnityEngine;
using System.Collections;

public static class TransformExtensions
{
    public static void LookAt2D(this Transform tr, Vector3 lookPos)
    {
        Vector3 difference = lookPos - tr.position;
        difference.Normalize();
        float z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        tr.rotation = Quaternion.Euler(0f, 0f, z - 90);
    }

    public static void LookAt2DLerp(this Transform tr, Vector3 lookPos, float t)
    {
        Vector3 difference = lookPos - tr.position;
        difference.Normalize();
        float z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        tr.rotation = Quaternion.Lerp(tr.rotation, Quaternion.Euler(0f, 0f, z - 90), t);
    }
}
