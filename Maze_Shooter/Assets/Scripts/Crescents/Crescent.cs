using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Arachnid;

public class Crescent : MonoBehaviour
{
	public Collection collection;
	public UnityEvent onCollected;
	public UnityEvent onActivated;

	static Collection activatedCrescents;

	void Activate() 
	{
		// activates this crescent group and de-activates all other crescents
		activatedCrescents = collection;

		// call events to move camera, put focus on other crescents, freeze frame, etc
		onActivated.Invoke();
	}

	public void OnTouched () 
	{
		// check if activated or not
		if (activatedCrescents == null) Activate();

		else if (activatedCrescents == collection) Collect();
	}
	

	void Collect() 
	{
		onCollected.Invoke();
	}

	public void MoveToGlyph() 
	{
		CrescentGroup group = CrescentGroup.CrescentGroupForCollection(collection);
		CrescentGlyph newGlyph = group.GetEmptyGlyph();
		StartCoroutine(LerpToGlyph(newGlyph));
	}

	IEnumerator LerpToGlyph(CrescentGlyph glyph) 
	{
		Vector3 startPos = transform.position;
		float progress = 0;
		
		while (progress < 1) {
			progress += Time.unscaledDeltaTime;
			transform.position = Vector3.Lerp(startPos, glyph.transform.position, progress);
			yield return null;
		}

		transform.parent = glyph.transform;
		transform.localPosition = Vector3.zero;
	}
}
