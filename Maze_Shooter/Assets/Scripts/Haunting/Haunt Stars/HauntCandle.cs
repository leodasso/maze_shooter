using UnityEngine;
using Sirenix.OdinInspector;

public class HauntCandle : MonoBehaviour
{
	public SpriteRenderer spriteRenderer;
	public SpaceMovement mover;
	public HauntCandleHolder slot;

	[Button]
	public void GotoSlot(HauntCandleHolder newSlot, float duration) 
	{
		slot = newSlot;
		mover.SetDestinationObject(slot.candlePosition);
		mover.PlayAnimation(0, 1, duration, FillSlot);
	}

	void FillSlot() 
	{
		if (slot) {
			mover.enabled = false;
			slot.FillSlot(gameObject);
		}
	}
}