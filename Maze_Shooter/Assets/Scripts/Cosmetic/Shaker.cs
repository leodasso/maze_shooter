using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;

public class Shaker : MonoBehaviour
{
	public Vector3 shakeIntensity = new Vector3(1, 1, 0);
	public float shakeFrequency = 5;

	[SerializeField]
	AnimationCurve autoShakeCurve = AnimationCurve.Linear(1, 1, 0, 0);

	Vector3 initPos;

	float totalShakeIntensity;
	float time;
	Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
		initPos = transform.localPosition;
		offset = Random.insideUnitSphere;
		totalShakeIntensity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (totalShakeIntensity < Mathf.Epsilon) return;

		time += Time.unscaledDeltaTime * shakeFrequency;
		Vector3 sineTime = offset + Vector3.one * time;

		Vector3 newPos = new Vector3(
			Mathf.Sin(sineTime.x),
			Mathf.Sin(sineTime.y),
			Mathf.Sin(sineTime.z)
		);

		transform.localPosition = initPos + Vector3.Scale(newPos, shakeIntensity) * totalShakeIntensity;
    }

	public void BeginShake()
	{
		totalShakeIntensity = 1;
	}

	public void StopShake()
	{
		totalShakeIntensity = 0;
	}

	[Button]
	public void DoShake()
	{
		DoShake(autoShakeCurve.Duration());
	}

	public void DoShake(float duration)
	{
		offset = Random.insideUnitSphere;
		StopAllCoroutines();
		StartCoroutine(ShakeSequence(duration));
	}

	IEnumerator ShakeSequence(float duration)
	{
		float progress = 0;
		float curveDuration = autoShakeCurve.Duration();
		float rate =  curveDuration / duration;

		while (progress < curveDuration)
		{
			totalShakeIntensity = autoShakeCurve.Evaluate(progress);
			progress += Time.unscaledDeltaTime * rate;
			yield return null;
		}
	}
}
