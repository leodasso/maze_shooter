using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathLegDriver : MonoBehaviour
{
	public float offset;
	public float pathToLegMovement = 1;
	public LegAnimator leg;
	public PathFollower pathFollower;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float pos = pathFollower.pathPosition + offset;
		pos *= pathToLegMovement;
		leg.progress = pos % 1;
    }
}
