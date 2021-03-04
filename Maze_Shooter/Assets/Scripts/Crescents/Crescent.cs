using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Arachnid;

public class Crescent : MonoBehaviour
{
	public PrettyLerper lerper;
	public Collection collection;
	
	public UnityEvent onCollected;
	public UnityEvent onActivated;

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
		lerper.target = glyph.transform;
		lerper.DoLerp();
		lerper.onLerpComplete += ActivateGlyph;
	}

	void ActivateGlyph() 
	{
		myGroup.ActivateGlyph(glyph);
		gameObject.SetActive(false);
	}
}