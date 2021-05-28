using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class RotationLink : MonoBehaviour
{
	[ToggleLeft, ValidateInput("ValidParentUsage", "Must have a parent in the hierarchy to use this option!")]
	public bool useParentAsMaster;

	[HideIf("useParentAsMaster")]
	public Transform customMasterTransform;

	[Range(0, 60), Tooltip("Show the rotation of the master x frames ago. (for a cool delay effect)")]
	public int framesOfDelay = 0;

	[Range(-1, 1)]
	public float weight = .95f;

	[SerializeField]
	Quaternion offsetRot;

	Quaternion initRot;

	Transform Master => useParentAsMaster ? transform.parent : customMasterTransform;

	bool ValidParentUsage => !useParentAsMaster || (transform.parent != null);

	List<Quaternion> masterRotations = new List<Quaternion>();

	Quaternion MasterRotation => framesOfDelay > 0 ? masterRotations[0] : Master.localRotation;

	[Button]
	void CaptureOffsetRotation()
	{
		offsetRot = Quaternion.Euler(transform.localEulerAngles - Master.localEulerAngles);
	}

    // Start is called before the first frame update
    void Start()
    {
		initRot = transform.localRotation;
		masterRotations.Add(Master.localRotation);
    }

    // Update is called once per frame
    void Update()
    {
        if (!Master) return;
 
		// Memorize frames of master rotation
		masterRotations.Add(Master.localRotation);
		if (masterRotations.Count > framesOfDelay)
			masterRotations.RemoveAt(0);

		transform.localRotation = Quaternion.LerpUnclamped(initRot, MasterRotation * offsetRot, weight);
    }
}
