using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class Masker : MonoBehaviour
{
	public GameObject mask;
	public UnityEvent onMaskEnable;
	public UnityEvent onMaskDisable;

	public void EnableMask(bool enable) 
	{
		if (enable) EnableMask();
		else DisableMask();
	}

	[ButtonGroup]
	public void EnableMask() 
	{
		mask.SetActive(true);
		onMaskEnable.Invoke();
	}

	[ButtonGroup]
	public void DisableMask() 
	{
		mask.SetActive(false);
		onMaskDisable.Invoke();
	}
}
