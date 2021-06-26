using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[AddComponentMenu("Shadows/Shadow Object")]
public class ShadowObject : MonoBehaviour
{
    [Tooltip("When calculating shadow color, the distance between caster and shadow will be clamped to this value.")]
    public float maxDistance = 10;
	public Transform mask;

	[ReadOnly]
	public float scale = 1;

	[ReadOnly]
	public bool isVisible = true;
    
    List<ShadowObjectPlugin> _plugins = new List<ShadowObjectPlugin>();
    float _dist;
    float _normalizedDist;
	float _actualScale;

	void Update()
	{
		float visibleScale = isVisible ? 1 : 0;

		_actualScale = Mathf.Lerp(_actualScale, visibleScale * scale, Time.unscaledDeltaTime * 12);
		mask.transform.localScale = Vector3.one * _actualScale;
	}

    public void SetDistance(float y)
    {
        _dist = Mathf.Abs(y - transform.position.y);
        _dist = Mathf.Clamp(_dist, 0, maxDistance);
        _normalizedDist = _dist / maxDistance;
        
        foreach (var plugin in _plugins)
        {
            plugin.Recalculate(Mathf.Clamp01(_normalizedDist));
        }
    }

    public void AddPlugin(ShadowObjectPlugin newPlugin)
    {
        _plugins.Add(newPlugin);
    }
}