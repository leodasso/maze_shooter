using UnityEngine;
using Sirenix.OdinInspector;

[TypeInfoBox("Scales to always look at the selected target of the target finder component" +
             " attached to this game object.")]
[RequireComponent(typeof(TargetFinder))]
public class FlipToTarget : SpriteFlipper
{
    TargetFinder _targetFinder;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _targetFinder = GetComponent<TargetFinder>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_targetFinder.currentTarget) return;
        UpdateScale( _targetFinder.currentTarget.transform.position.x - transform.position.x);
    }
}
