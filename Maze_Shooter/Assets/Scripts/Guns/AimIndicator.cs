using UnityEngine;
using Arachnid;

public class AimIndicator : MonoBehaviour
{
    public GunBrain gunBrain;
    public SpriteRenderer indicatorSprite;
    
    [Tooltip("How far does the player's joystick need to lean before we prep a red devil for launch? (between 0 and 1)")]
    public FloatReference inputThreshhold;
    
    Color _color;
    float _inputMagnitude;
    
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
        _inputMagnitude = gunBrain.fireInput.magnitude;
        if (_inputMagnitude < inputThreshhold.Value)
            SetAlpha(0);
        else
            SetAlpha(_inputMagnitude);
        transform.rotation = gunBrain.aim;

    }

    void OnDisable()
    {
        SetAlpha(0);
    }
}
