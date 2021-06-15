using System;
using UnityEngine;
using UnityEngine.Events;
using PlayMaker;

[ExecuteAlways]
public class ArcMover : MonoBehaviour
{
    [Range(0, 1)]
    public float progress = 0;
    public Vector3 startPosition;
    public GameObject end;
	[SerializeField]
    AnimationCurve heightCurve;
	[SerializeField]
	PlayMakerFSM playMaker;

    public UnityEvent transitionIn;
    public UnityEvent transitionOut;

    public TrailRenderer trailRenderer;

    public Action onTransitionComplete;

    float _y;

	Vector3 endPos;

    // Update is called once per frame
    void Update()
    {
        if (end) 
			endPos = end.transform.position;
        _y = heightCurve.Evaluate(progress);
        transform.position = Vector3.Lerp(startPosition, endPos, progress) + Vector3.up * _y;
    }

	public void SetTransitionDuration(float duration) 
	{
		playMaker.FsmVariables.GetFsmFloat("transitionDuration").Value = duration;

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