using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[ExecuteAlways]
public class FollowObject : MonoBehaviour
{
	[HorizontalGroup, LabelWidth(20), ToggleLeft]
	public bool x = true;

	[HorizontalGroup, LabelWidth(20), ToggleLeft]
	public bool y = true;

	[HorizontalGroup, LabelWidth(20), ToggleLeft]
	public bool z = true;

    public GameObject objectToFollow;
    public Vector3 offset;

    // Update is called once per frame
    void LateUpdate()
    {
        if (!objectToFollow) return;

		Vector3 followPos = objectToFollow.transform.position + offset;

		Vector3 newPos = new Vector3 (
			x ? followPos.x : transform.position.x,
			y ? followPos.y : transform.position.y,
			z ? followPos.z : transform.position.z
		);


        transform.position = newPos;
    }
}
