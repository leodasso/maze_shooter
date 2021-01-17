using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class HeartsTester : MonoBehaviour
{

	public HealthPoints value;

	public HealthPoints input;
    // Start is called before the first frame update
    void Start()
    {
        
    }

	[ButtonGroup("1")]
	void Add() {
		value += 1;
	}

	[ButtonGroup("1")]
	void Subtract() {
		value -= 1;
	}

	[ButtonGroup("2")]
	void AddInput() {
		value += input;
	}

	[ButtonGroup("2")]
	void SubtractInput() {
		value -= input;
	}

	[Button]
	void SetValue(int qty) {
		value = qty;
	}
}
