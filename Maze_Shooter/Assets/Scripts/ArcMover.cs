using System;
using UnityEngine;
using UnityEngine.Events;

[ExecuteAlways]
public class ArcMover : MonoBehaviour
{
    [Range(0, 1)]
    public float progress = 0;
    public Vector3 startPosition;
    public GameObject end;
    public AnimationCurve heightCurve;

    public UnityEvent transitionIn;
    public UnityEvent transitionOut;

    public TrailRenderer trailRenderer;

    public Action onTransitionComplete;

    float _y;

    // Update is called once per frame
    void Update()
    {
        if (!end) return;
        _y = heightCurve.Evaluate(progress);
        transform.position = Vector3.Lerp(startPosition, end.transform.position, progress) + Vector3.up * _y;
    }

    // Below functions are for easy interfacing with PlayMaker
    public void BeginTrailEmit()
    {
        trailRenderer.emitting = true;
    }

    public void StopTrailEmit()
    {
        trailRenderer.emitting = false;
    }

    public void TransitionComplete()
    {
        if (onTransitionComplete != null)
            onTransitionComplete.Invoke();
    }
}