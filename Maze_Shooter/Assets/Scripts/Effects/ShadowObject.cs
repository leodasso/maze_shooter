using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ShadowObject : MonoBehaviour
{
    [Tooltip("When calculating shadow color, the distance between caster and shadow will be clamped to this value.")]
    public float maxDistance = 10;
    public Gradient colorByDistance;
    public List<SpriteRenderer> sprites = new List<SpriteRenderer>();

    float _dist;
    float _normalizedDist;

    [Button]
    void GetAllSprites()
    {
        sprites.Clear();
        sprites.AddRange(GetComponentsInChildren<SpriteRenderer>());
    }

    public void SetDistance(float y)
    {
        _dist = Mathf.Abs(y - transform.position.y);
        _dist = Mathf.Clamp(_dist, 0, maxDistance);
        _normalizedDist = _dist / maxDistance;
        SetColor(colorByDistance.Evaluate(_normalizedDist));
    }

    void SetColor(Color color)
    {
        foreach (var sprite in sprites)
            sprite.color = color;
    }
}
