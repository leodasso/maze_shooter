using UnityEngine;
using Arachnid;

[ExecuteAlways]
public class FlatRotateToTarget : MonoBehaviour
{
	public Transform target;
	public float degreesOffset;

    void Update()
    {
        if (!target) return;

		Vector3 vectorToTarget = target.transform.position - transform.position;
		float angle = Math.AngleFromVector2(vectorToTarget, degreesOffset);
		transform.localEulerAngles = new Vector3(0, 0, angle);
    }
}