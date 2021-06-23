using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class HauntCandle : MonoBehaviour
{
	[SerializeField]
	SpriteRenderer spriteRenderer;

	[SerializeField]
	SpaceMovement mover;
	
	public HauntCandleHolder slot;

	[SerializeField]
	UnityEvent onPutInSlot;

	[Button]
	public void GotoSlot(HauntCandleHolder newSlot, float duration) 
	{
		slot = newSlot;
		mover.SetDestinationObject(slot.candlePosition);
		mover.PlayAnimation(0, 1, duration, FillSlot);
		onPutInSlot.Invoke();
	}

	void FillSlot() 
	{
		if (slot) {
			mover.enabled = false;
			slot.FillSlot(gameObject);
		}
	}
}