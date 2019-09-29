using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class FollowObject : MonoBehaviour
{

    public GameObject objectToFollow;
    public Vector3 offset;

    // Update is called once per frame
    void LateUpdate()
    {
        if (!objectToFollow) return;
        transform.position = objectToFollow.transform.position + offset;
    }
}
