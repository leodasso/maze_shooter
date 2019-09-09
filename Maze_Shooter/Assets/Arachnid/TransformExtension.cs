using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtension
{
    public static int ActiveChildCount(this Transform transform)
    {
        int c = 0;
        foreach (Transform t in transform)
        {
            if (t.gameObject.activeSelf) c++;
        }

        return c;
    }
}
