using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ActionTaker : MonoBehaviour
{
	[ShowInInspector]
	HashSet<ActionPoint> notifyPoints = new HashSet<ActionPoint>();

	[ShowInInspector]
	HashSet<ActionPoint> actionablePoints = new HashSet<ActionPoint>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var point in ActionPoint.allActionPoints) 
		{
			if (IsInNotifyRange(point)) 
			{
				AddToNotifyList(point);
				if (IsInActionRange(point)) 
					AddToActionList(point);
				else 
					RemoveFromActionList(point);
					
			}
			else RemoveFromNotifyList(point);
		}
    }


	void AddToNotifyList(ActionPoint point) 
	{
		if (notifyPoints.Add(point))
			point.Notify();
	}

	void RemoveFromNotifyList(ActionPoint point) 
	{
		if (notifyPoints.Remove(point))
			point.DeNotify();

		RemoveFromActionList(point);
	}

	void AddToActionList ( ActionPoint point) 
	{
		if (actionablePoints.Add(point))
			point.ShowAction();
	}

	void RemoveFromActionList (ActionPoint point) 
	{
		if (actionablePoints.Remove(point))
			point.HideAction();
	}

	bool IsInNotifyRange(ActionPoint point) 
	{
		return IsInRange(point.transform.position, point.notifyDistance);
	}

	bool IsInActionRange(ActionPoint point) 
	{
		if (!point.hasAction) return false;
		return IsInRange(point.transform.position, point.actionDistance);
	}

	bool IsInRange(Vector3 otherPoint, float minRange) 
	{
		float square = minRange * minRange;
		return Vector3.SqrMagnitude(transform.position - otherPoint) <= square;
	}
}
