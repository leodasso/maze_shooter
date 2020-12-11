using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HauntStarSlot : MonoBehaviour
{
	public SpaceMovement mover;
	public UnityEvent onFilled;


    // Start is called before the first frame update
    void Start()
    {

    }

	public void FillSlot() 
	{
		onFilled.Invoke();
	}

	public void PlayAnimation(float beginningValue, float endValue, float duration) 
	{
		mover.PlayAnimation(beginningValue, endValue, duration);
	}
}