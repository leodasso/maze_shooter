using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class ActionPoint : MonoBehaviour
{

	[Tooltip("Local position where the notification gui will appear")]
	public Vector3 guiPosition = Vector3.up;

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

	[ShowInInspector, ReadOnly]
	ActionGui actionGuiInstance;

	static GameObject actionIconsPrefab;

	public static HashSet<ActionPoint> allActionPoints = new HashSet<ActionPoint>();

	static void LoadActionIconsPrefab()
	{
		if (actionIconsPrefab) return;
		actionIconsPrefab = Resources.Load("action icons") as GameObject;
		Debug.Log(actionIconsPrefab.name, actionIconsPrefab);
	}

	void OnDrawGizmos() 
	{
		Gizmos.color = new Color(1, 1, 0, .2f);
		GizmoExtensions.DrawCircle(transform.position, notifyDistance);

		Gizmos.color = new Color(1, 1, 0, .8f);
		GizmoExtensions.DrawCircle(transform.position, actionDistance);

		Gizmos.DrawSphere(transform.position + guiPosition, .1f);
	}

    void Awake()
    {
        allActionPoints.Add(this);
		LoadActionIconsPrefab();
		InstantiateGui();
    }

	void InstantiateGui() 
	{
		GameObject newInstance = Instantiate(actionIconsPrefab, transform.position + guiPosition, Quaternion.identity);
		newInstance.transform.parent = transform;

		actionGuiInstance = newInstance.GetComponent<ActionGui>();
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
