using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneGrass : MonoBehaviour
{
	[SerializeField]
	Transform target;

	List<Vector3> offsets = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        // generate offsets
		for (int i = 0; i < transform.childCount; i++) {
			Vector3 newOffet = transform.GetChild(i).position - target.position;
			offsets.Add(Vector3.Scale(newOffet, new Vector3(1, 0, 1)));
		}
    }

    // Update is called once per frame
    void Update()
    {
		for (int i = 0; i < transform.childCount; i++) {
			Vector3 upwards =  transform.GetChild(0).position - (target.position + offsets[i]);
			transform.GetChild(i).rotation = Quaternion.LookRotation(Vector3.forward, upwards);
		}
    }
}
