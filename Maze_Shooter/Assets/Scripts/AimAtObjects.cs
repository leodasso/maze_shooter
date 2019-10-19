using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Arachnid;
using Sirenix.OdinInspector;

public class AimAtObjects : MonoBehaviour
{
	[ToggleLeft, Tooltip("By default, searches for target finder component on this object. If you toggle this," +
	                     " you can select any target finder.")]
	public bool chooseTargetFinder;

	[ShowIf("chooseTargetFinder")]
	public TargetFinder customTargetFinder;

	[Tooltip("Higher numbers mean it will closely aim towards target. Lower numbers mean it can lag behind."), MinValue(.01f)]
	public float aimQuickness = 20;
	
	TargetFinder _targetFinder;

	TargetFinder selectedTargetFinder => chooseTargetFinder ? customTargetFinder : _targetFinder;

	Vector3 _targetDelta;
	
	// Use this for initialization
	void Start ()
	{
		_targetFinder = GetComponent<TargetFinder>();
		_targetDelta = transform.position;
	}

	// Update is called once per frame
	void Update ()
	{
		if (selectedTargetFinder == null) return;
		if (selectedTargetFinder.currentTarget == null) return;

		_targetDelta = Vector3.Lerp(_targetDelta, selectedTargetFinder.currentTarget.transform.position - transform.position,
			Time.deltaTime * aimQuickness);

		float z = Math.AngleFromVector2(_targetDelta, -90);
		transform.eulerAngles = new Vector3(0, 0, z);
	}
}
