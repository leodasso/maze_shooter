using UnityEngine;

public class SpriteFlipper : MonoBehaviour
{
    public Transform objectToScale;
    float _defaultScaleX;
    int _facingDirection = 1;
    int _prevDirection = 99;
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        _defaultScaleX = objectToScale.localScale.x;
    }

    protected void UpdateScale(float direction)
    {
        _facingDirection = direction >= 0 ? 1 : -1;
        if (_facingDirection == _prevDirection) return;
        objectToScale.localScale = new Vector3(_defaultScaleX * _facingDirection, objectToScale.localScale.y, 1);
        _prevDirection = _facingDirection;
    }
}