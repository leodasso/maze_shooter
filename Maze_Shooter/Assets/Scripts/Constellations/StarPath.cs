using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
using Sirenix.OdinInspector;

[ExecuteAlways]
public class StarPath : MonoBehaviour
{
	public Transform objectOnPath;
	public Transform starSlot;
	public CinemachineSmoothPath path;
	[Space]
	public AnimationCurve movementCurve = AnimationCurve.Linear(0, 0, 1, 1);

	[Space]
	public bool scaleObjectOnPath;
	[ShowIf("scaleObjectOnPath")]
	public AnimationCurve scaleCurve = AnimationCurve.Linear(0, 0, 1, 1);

	public bool unscaledTime;

	[Range(0, 1)]
	public float progress;

	[ReadOnly, ShowInInspector]
	int lastPoint = 0;

	public UnityEvent moveAlongPathComplete;

	float deltaTime => unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

    // Update is called once per frame
    void Update()
    {
        if (starSlot && path) {
			lastPoint = path.m_Waypoints.Length - 1;
			path.m_Waypoints[lastPoint].position = transform.InverseTransformPoint(starSlot.position);
		}

		if (objectOnPath) {
			float dist = path.PathLength * progress;
			objectOnPath.position = path.EvaluatePositionAtUnit(dist, CinemachinePathBase.PositionUnits.Distance);
		}
    }

	[Button]
	public void MoveAlongPath() {
		StartCoroutine(MoveAlongPathSequence());
	}

	IEnumerator MoveAlongPathSequence() 
	{
		Vector3 initScale = objectOnPath.transform.localScale;
		float lerpProgress = 0;
		float duration = movementCurve.Duration();
		
		while (lerpProgress < 1) {
			lerpProgress += deltaTime / duration;
			progress = movementCurve.Evaluate(lerpProgress * duration);
			if (scaleObjectOnPath)
				objectOnPath.transform.localScale = initScale * scaleCurve.Evaluate(lerpProgress);
			yield return null;
		}
		progress = 1;
		moveAlongPathComplete.Invoke();
	}

	public void setStartPosition(Transform t) {
		path.m_Waypoints[0].position = transform.InverseTransformPoint(t.position);
	}
}
