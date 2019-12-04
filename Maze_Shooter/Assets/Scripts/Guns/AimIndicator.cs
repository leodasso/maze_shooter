using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimIndicator : MonoBehaviour
{
    public GunBrain gunBrain;
    public SpriteRenderer indicatorSprite;

    Color _color;
    
    // Start is called before the first frame update
    void Start()
    {
        SetAlpha(0);
    }

    void SetAlpha(float alpha)
    {
        _color = indicatorSprite.color;
        indicatorSprite.color = new Color(_color.r, _color.g, _color.b, alpha);
    }

    // Update is called once per frame
    void Update()
    {
        SetAlpha(gunBrain.fireInput.magnitude);
        transform.rotation = gunBrain.aim;

    }
}
