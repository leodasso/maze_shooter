using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Arachnid;
using Cinemachine;

public class Crescent : MonoBehaviour
{
	public Collection collection;

	[SerializeField]
	CrescentPath pathPrefab;

	[SerializeField]
	PathFollower pathFollower;

	[SerializeField]
	UnityEvent onCollected;
	[SerializeField]
	UnityEvent onActivated;

	[SerializeField]
	[Tooltip("Curve for the progress of the crescent along the path (when you pick it up)" +
	"\n x-axis: time \n y-axis: progress along path")]
	AnimationCurve pathMovementCurve = AnimationCurve.Linear(0, 0, 1, 1);

	CrescentGroup myGroup;
	CrescentGlyph glyph;

	static Collection activatedCrescents;

	void Start() 
	{
		myGroup = CrescentGroup.CrescentGroupForCollection(collection);
	}

	void Activate() 
	{
		// activates this crescent group and de-activates all other crescents
		activatedCrescents = collection;

		// call events to move camera, put focus on other crescents, freeze frame, etc
		onActivated.Invoke();
		if (myGroup) myGroup.onActivated.Invoke();
	}

	public void OnTouched () 
	{
		if (activatedCrescents == null) 
			Activate();

		if (activatedCrescents == collection) 
			Collect();
	}
	

	void Collect() 
	{
		onCollected.Invoke();
	}

	public void MoveToGlyph() 
	{
		glyph = myGroup.GetEmptyGlyph();

		// Instantiate path
		CrescentPath pathInstance = Instantiate(pathPrefab, transform.position, Quaternion.identity);
		pathInstance.SetupPath(this, glyph);
		pathFollower.pathPosition = 0;
		StartCoroutine(MoveAlongPath(pathInstance.GetComponent<CinemachinePath>()));
	}

	IEnumerator MoveAlongPath(CinemachinePath path) 
	{
		float duration = pathMovementCurve.Duration();
		float progress = 0;
		Debug.Log("Starting lerp with a duration of " + duration);

		pathFollower.path = path;

		while (progress < 1) {
			progress += Time.deltaTime / duration;
			Debug.Log("progress is now " + progress);
			pathFollower.SetNormalizedPathPos(pathMovementCurve.Evaluate(progress * duration));
			yield return null;
		}

		ActivateGlyph();
	}

	void ActivateGlyph() 
	{
		myGroup.ActivateGlyph(glyph);
		gameObject.SetActive(false);
	}
}