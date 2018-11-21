using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCrystal : MonoBehaviour
{
	public Stage stage;
	Animator _animator;

	// Use this for initialization
	void Start ()
	{
		_animator = GetComponent<Animator>();
	}

	public void SetSelected(bool selected)
	{
		_animator.SetBool("selected", selected);
	}
}
