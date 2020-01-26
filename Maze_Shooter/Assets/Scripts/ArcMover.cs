using UnityEngine;

[ExecuteAlways]
public class ArcMover : MonoBehaviour
{
    [Range(0, 1)]
    public float progress = 0;
    public Vector3 startPosition;
    public GameObject end;
    public AnimationCurve heightCurve;

    float _y;

    // Update is called once per frame
    void Update()
    {
        if (!end) return;
        _y = heightCurve.Evaluate(progress);
        transform.position = Vector3.Lerp(startPosition, end.transform.position, progress) + Vector3.up * _y;
    }
}