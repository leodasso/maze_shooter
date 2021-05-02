using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[AddComponentMenu("Animation/Leg Animator")]
public class LegAnimator : MonoBehaviour
{
	[Range(0, 1)]
	public float progress = .5f;
	public AnimationCurve heightOverProgress = AnimationCurve.Linear(0, 0, 1, 1);
	public AnimationCurve zOverProgress = AnimationCurve.Linear(0, 0, 1, 1);

	public float thighLength = .5f;
	public float shinLength = .5f;

	public LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (lineRenderer) {
			
			float z = zOverProgress.Evaluate(progress);
			float height = heightOverProgress.Evaluate(progress);

			Vector3 kneePoint = new Vector3(thighLength, height, z);
			Vector3 footPoint = new Vector3(thighLength, height - shinLength, z);

			lineRenderer.positionCount = 3;
			lineRenderer.SetPosition(1, kneePoint);
			lineRenderer.SetPosition(2, footPoint);
		}
    }
}
