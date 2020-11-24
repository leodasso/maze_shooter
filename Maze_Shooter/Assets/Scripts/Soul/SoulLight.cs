using UnityEngine;
using Sirenix.OdinInspector;

[TypeInfoBox("Soul lights restore the soul of anything they touch.")]
public class SoulLight : MonoBehaviour
{
    public float intensity = 1;
    float _distance = 0;
    float _normalizedDist;

    [Tooltip("The x axis is normalized distance from center (0-1), and y axis is intensity multiplier.")]
    public AnimationCurve intensityCurve;
    public new SphereCollider collider;

	// leave this in so the little intensity toggle thing is available
	void Start() {

	}

    /// <summary>
    /// Returns the intensity of light at the given point
    /// </summary>
    public float LightIntensity(Vector3 point)
    {
		if (!enabled) return 0;
        _distance = Vector3.Distance(point, transform.position);
        _normalizedDist = _distance / (collider.radius * ScaleMagnitude());
        return intensity * intensityCurve.Evaluate(_normalizedDist);
    }

    float ScaleMagnitude()
    {
        return transform.lossyScale.x;
    }
}
