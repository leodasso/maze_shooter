using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[TypeInfoBox("Scales to always look at the selected target of the target finder component" +
             " attached to this game object.")]
[RequireComponent(typeof(TargetFinder))]
public class ScaleToFaceTarget : MonoBehaviour
{
    public Transform objectToScale;
    TargetFinder _targetFinder;
    float _defaultScaleX;
    int direction = 1;
    int prevDirection = 99;
    
    // Start is called before the first frame update
    void Start()
    {
        _defaultScaleX = objectToScale.localScale.x;
        _targetFinder = GetComponent<TargetFinder>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_targetFinder.currentTarget) return;

        // Set to be - 1 if the target is to the left of this; 1 otherwise
        direction = _targetFinder.currentTarget.transform.position.x >= transform.position.x ? 1 : -1;

        if (direction != prevDirection)
        {
            objectToScale.localScale = new Vector3(_defaultScaleX * direction, objectToScale.localScale.y, 1);
            prevDirection = direction;
        }
    }
}
