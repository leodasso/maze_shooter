using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
using Sirenix.OdinInspector;

public class Interior : MonoBehaviour
{

	[SerializeField]
	UnityEvent onEnter;

	[SerializeField]
	UnityEvent onExit;

	[Space]
	[SerializeField, Tooltip("(optional) place mask(s) here to have it be opened/closed at the right time. \n " +
			"if you want more custom actions for the masker, leave this blank and use the onEnter and onExit events.")]
	List<Scaler> maskScalers;

	[SerializeField]
	[Tooltip("(optional) if there's a camera associated with this interior, ref here to have the priority auto-controlled." + 
		"\n updates priority on enter/exit events.")]
	CinemachineVirtualCamera virtualCamera;

	[MinValue(1), SerializeField, ShowIf("HasVirtualCam")]
	[Tooltip("Priority that the cinemachine virtual cam will have when the interior is showing")]
	public int activeCamPriority = 10;

	bool HasVirtualCam => virtualCamera != null;

	public void OnEnter()
	{
		onEnter.Invoke();

		for (int i = 0; i < maskScalers.Count; i++)
			maskScalers[i].ScaleToBig();

		if (virtualCamera)
			virtualCamera.Priority = activeCamPriority;
	}

	public void OnExit() 
	{
		onExit.Invoke();

		for (int i = 0; i < maskScalers.Count; i++)
			maskScalers[i].ScaleToSmall();

		if (virtualCamera)
			virtualCamera.Priority = 0;

	}
}
