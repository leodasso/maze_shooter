using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Arachnid;

public class CheckSavedBool : MonoBehaviour
{
	public BoolValue savedBool;
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
		if (savedBool.Value) isTrue.Invoke();
		else isFalse.Invoke();
	}
}
