using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HauntStarSlot : MonoBehaviour
{
	public SpaceMovement mover;
	public LerpMovement lerpMovement;
	public UnityEvent onFilled;
	public UnityEvent onFailedToFill;

	bool _filled;

    // Start is called before the first frame update
    void Start()
    {
    }

	public void FillSlot() 
	{
		_filled = true;
		onFilled.Invoke();
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
		mover.enabled = false;
		lerpMovement.target = transform.parent;
	}
}