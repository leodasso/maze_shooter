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
        // Using the absolute value here so that 1 can always be associated with 'right' even if
        // the sprite starts off scaled left
        _defaultScaleX = Mathf.Abs(objectToScale.localScale.x);
    }

    protected void UpdateScale(float direction)
    {
        _facingDirection = direction >= 0 ? 1 : -1;
        if (_facingDirection == _prevDirection) return;
        objectToScale.localScale = new Vector3(_defaultScaleX * _facingDirection, objectToScale.localScale.y, 1);
        _prevDirection = _facingDirection;
    }

    void OnDisable()
    {
        _prevDirection = 99;
    }
}