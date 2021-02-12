using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class ActionPoint : MonoBehaviour
{
	[Tooltip("If the player is within this distance, a notification icon will show")]
	public float notifyDistance = 10;

	[ToggleLeft]
	public bool hasAction = true;

	[ShowIf("hasAction")]
	[Tooltip("If the player is within this distance, the action button will trigger this")]
	public float actionDistance = 1;

	[ShowIf("hasAction")]
	[SerializeField]
	UnityEvent onAction;

	[SerializeField]
	ActionGui actionGuiInstance;

	public static HashSet<ActionPoint> allActionPoints = new HashSet<ActionPoint>();

	void OnDrawGizmos() 
	{
		Gizmos.color = new Color(1, 1, 0, .2f);
		GizmoExtensions.DrawCircle(transform.position, notifyDistance);

		Gizmos.color = new Color(1, 1, 0, .8f);
		GizmoExtensions.DrawCircle(transform.position, actionDistance);
	}

    void Awake()
    {
        allActionPoints.Add(this);
    }

	void OnDestroy()
	{
		allActionPoints.Remove(this);
	}

	public void Notify() 
	{
		actionGuiInstance.Notify();
	}

	public void DeNotify() 
	{
		actionGuiInstance.DeNotify();
	}

	public void ShowAction() 
	{
		actionGuiInstance.ShowAction();
	}

	public void HideAction() 
	{
		actionGuiInstance.HideAction();
	}
}
