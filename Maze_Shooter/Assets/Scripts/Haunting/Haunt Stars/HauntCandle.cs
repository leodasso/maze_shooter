using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class HauntCandle : MonoBehaviour
{
	public SpriteRenderer spriteRenderer;
	public SpaceMovement mover;
	public HauntCandleHolder slot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

	[Button]
	public void GotoSlot(HauntCandleHolder newSlot, float duration) 
	{
		slot = newSlot;
		mover.SetDestinationObject(slot.transform);
		mover.PlayAnimation(0, 1, duration, FillSlot);
	}

	void FillSlot() 
	{
		if (slot) slot.FillSlot();
		spriteRenderer.enabled = false;
	}
}