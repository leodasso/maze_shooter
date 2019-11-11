using UnityEngine;
using Sirenix.OdinInspector;

[ExecuteAlways]
public class ArcMover : MonoBehaviour
{

    [Range(0, 1)]
    public float progress = 0;
    public Vector3 startPosition;
    public GameObject end;
    
    public PseudoDepth myDepth;

    [ShowIf("HasPseudoDepth")]
    public AnimationCurve heightCurve;

    public bool HasPseudoDepth => myDepth != null;

    // Update is called once per frame
    void Update()
    {
        if (!end) return;

        transform.position = Vector2.Lerp(startPosition, end.transform.position, progress);
        if (HasPseudoDepth)
            myDepth.z = heightCurve.Evaluate(progress);
    }
}
