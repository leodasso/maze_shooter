
using UnityEngine;

public class LerpMovement : MonoBehaviour
{
	public Transform target;
	public float lerpSpeed = 10;
	public bool useRealtime = false;

    // Update is called once per frame
    void Update()
    {
        if (!target) return;
		float t = useRealtime ? Time.unscaledDeltaTime : Time.deltaTime;
		transform.position = Vector3.Lerp(transform.position, target.position, t * lerpSpeed);
    }
}
