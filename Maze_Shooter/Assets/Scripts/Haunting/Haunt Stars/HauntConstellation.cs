using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class HauntConstellation : MonoBehaviour
{

	[Tooltip("How long to wait (in seconds) between showing each haunt star slot")]
	public float delayBetweenSlots = .1f;
	public float slotAnimDuration = .5f;
	public List<HauntStarSlot> hauntStarSlots = new List<HauntStarSlot>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	[Button]
	void GetHauntStarSlots() {
		hauntStarSlots.Clear();
		hauntStarSlots.AddRange(GetComponentsInChildren<HauntStarSlot>());
	}

	[Button]
	public void ShowSlots() 
	{
		StartCoroutine(ShowSlotsSequence());
	}

	IEnumerator ShowSlotsSequence() 
	{
		int slotIndex = 0;

		while (slotIndex < hauntStarSlots.Count) {
			yield return new WaitForSecondsRealtime(delayBetweenSlots);
			hauntStarSlots[slotIndex].PlayAnimation(0, 1, slotAnimDuration);
			slotIndex++;
		}
	}
}
