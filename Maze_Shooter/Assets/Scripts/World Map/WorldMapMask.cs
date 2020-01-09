using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class WorldMapMask : MonoBehaviour
{
    [Tooltip("How big should the mask be when activated?")]
    [OnValueChanged("UpdateScale")]
    public float maskSize = 1.5f;

    [Tooltip("How quickly does the mask appear and disappear?")]
    public float transitionSpeed = 10;
    SpriteMask _spriteMask;

    float _scale;
    float _desiredScale = 0;

    void Awake()
    {
        _spriteMask = GetComponent<SpriteMask>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _spriteMask.enabled = false;
        transform.localScale = Vector3.zero;
        _scale = 0;
        _desiredScale = 0;
    }

    public void UpdateScale()
    {
        transform.localScale = Vector3.one * maskSize;
    }

    [Button]
    public void Appear()
    {
        _desiredScale = maskSize;
    }

    [Button]
    public void Disappear()
    {
        _desiredScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _scale = Mathf.Lerp(_scale, _desiredScale, Time.unscaledDeltaTime * transitionSpeed);
        transform.localScale = Vector3.one * _scale;

        if (_spriteMask.enabled && _scale < .05f) _spriteMask.enabled = false;

        if (!_spriteMask.enabled && _scale > .05f) _spriteMask.enabled = true;
    }
}
