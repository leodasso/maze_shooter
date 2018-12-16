using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;

public class SetSpringJointConnectedBody : FilteredTrigger2D
{
    public SpringJoint2D springJoint2D;

    protected override void OnTriggered(Collider2D triggerer)
    {
        Rigidbody2D rb = triggerer.GetComponent<Rigidbody2D>();
        if (!rb) return;
        springJoint2D.connectedBody = rb;
    }
}
