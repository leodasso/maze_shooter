using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class MaskerGroup : MonoBehaviour
{
	[Tooltip("Delay between activating each masker in the list")]
	public float activationDelay = .1f;
	public List<Masker> maskers = new List<Masker>();

	public UnityEvent onSequenceComplete;

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

	[ButtonGroup("reverse")]
	public void EnableMasksReverse() 
	{
		if (!Application.isPlaying) return;
		StartCoroutine(MaskSequence(true, true));
	}

	[ButtonGroup("reverse")]
	public void DisableMasksReverse() 
	{
		if (!Application.isPlaying) return;
		StartCoroutine(MaskSequence(false, true));
	}


	IEnumerator MaskSequence(bool enable, bool reverse = false) 
	{
		List<Masker> tempList = new List<Masker>(maskers);
		if (reverse)
			tempList.Reverse();

		for (int i = 0; i < tempList.Count; i++) {
			tempList[i].EnableMask(enable);
			yield return new WaitForSecondsRealtime(activationDelay);
		}

		onSequenceComplete.Invoke();
	}

	[Button]
	void GetMaskerChildren() 
	{
		maskers.Clear();
		maskers.AddRange(GetComponentsInChildren<Masker>());
	}
}