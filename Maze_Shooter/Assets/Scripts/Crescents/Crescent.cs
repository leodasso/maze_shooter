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

	CrescentGroup myGroup;

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
		CrescentGlyph newGlyph = myGroup.GetEmptyGlyph();
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

		myGroup.ActivateGlyph(glyph);
		gameObject.SetActive(false);
	}
}
