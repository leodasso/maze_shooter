using UnityEngine;
using UnityEngine.Events;

public class HauntCandleHolder : MonoBehaviour
{
	[SerializeField]
	SpaceMovement mover;

	[SerializeField]
	UnityEvent onFilled;

	[SerializeField]
	UnityEvent onFailedToFill;

	[SerializeField]
	UnityEvent onRecall;

	[SerializeField]
	Transform candlePosition;

	[Space, SerializeField]
	float floatAwaySpeed = 1;

	bool _filled;


	public void FillSlot(GameObject candle) 
	{
		_filled = true;
		onFilled.Invoke();

		candle.transform.parent = candlePosition;
		candle.transform.localPosition = Vector3.zero;
	}

	public bool CheckIfFilled() 
	{
		if (!_filled) onFailedToFill.Invoke();
		return _filled;
	}

	public void PlayAnimation(float beginningValue, float endValue, float duration) 
	{
		mover.PlayAnimation(beginningValue, endValue, duration);
	}

	public void Recall() 
	{
		onRecall.Invoke();
	}
}