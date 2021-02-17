using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arachnid;

public class CrescentGroup : MonoBehaviour
{
	public Collection crescentCollection;
	public List<Transform> crescentSlots = new List<Transform>();

	static HashSet<CrescentGroup> crescentGroups = new HashSet<CrescentGroup>();

	List<Transform> availableSlots = new List<Transform>();

	public static CrescentGroup CrescentGroupForCollection(Collection collection) 
	{
		foreach( CrescentGroup g in crescentGroups) {
			if (g.crescentCollection == collection) return g;
		}
		return null;
	}

    void Awake()
    {
        crescentGroups.Add(this);
		availableSlots = new List<Transform>(crescentSlots);
    }

	void OnDestroy()
	{
		crescentGroups.Remove(this);
	}

	public Transform GetSlot() 
	{
		Transform slot = availableSlots[0];
		availableSlots.RemoveAt(0);
		return slot;
	}

}
