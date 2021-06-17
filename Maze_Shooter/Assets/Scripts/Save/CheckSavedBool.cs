using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckSavedBool : MonoBehaviour
{
	public SavedBool savedBool;
	public bool checkOnStart = true;

	public UnityEvent isTrue;
	public UnityEvent isFalse;

    // Start is called before the first frame update
    void Start()
    {
        if (checkOnStart) Check();
    }

	public void Check() 
	{
		if (savedBool.runtimeValue) isTrue.Invoke();
		else isFalse.Invoke();
	}
}
