using UnityEngine;
using Sirenix.OdinInspector;
using Arachnid;

[TypeInfoBox("Soul lights restore the soul of anything they touch.")]
public class SoulLight : MonoBehaviour
{
    float _distance = 0;
    public new SphereCollider collider;

    /// <summary>
    /// Returns if the given point is lit by this soul light
    /// </summary>
    public bool DoesLightPoint(Vector3 point)
    {
        return Math.IsInRange(transform.position - point, collider.radius);
    }
}