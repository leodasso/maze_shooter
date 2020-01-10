using UnityEngine;
using Sirenix.OdinInspector;

[TypeInfoBox("On start, unparents from the transform parent. Follows former parent's position.")]
public class UnparentAndFollow : MonoBehaviour
{
    Vector3 _offset;
    Transform _formerParent;
    
    // Start is called before the first frame update
    void Start()
    {
        _formerParent = transform.parent;
        _offset = transform.localPosition;
        transform.parent = null;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!_formerParent) return;
        transform.position = _formerParent.transform.position + _offset;
    }
}
