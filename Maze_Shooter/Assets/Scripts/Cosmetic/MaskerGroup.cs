using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class MaskerGroup : MonoBehaviour
{
	[Tooltip("Delay between activating each masker in the list")]
	public float activationDelay = .1f;
	public List<Masker> maskers = new List<Masker>();

	[ButtonGroup]
	public void EnableMasks() 
	{
		if (!Application.isPlaying) return;
		StartCoroutine(MaskSequence(true));
	}

	[ButtonGroup]
	public void DisableMasks() 
	{
		if (!Application.isPlaying) return;
		StartCoroutine(MaskSequence(false));
	}

	IEnumerator MaskSequence(bool enable) 
	{
		for (int i = 0; i < maskers.Count; i++) {
			if (enable) 
				maskers[i].EnableMask();
			else 
				maskers[i].DisableMask();

			yield return new WaitForSecondsRealtime(activationDelay);
		}
	}

	[Button]
	void GetMaskerChildren() 
	{
		maskers.Clear();
		maskers.AddRange(GetComponentsInChildren<Masker>());
	}
}