using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpringJoint2D)), RequireComponent(typeof(LineRenderer))]
public class SpringJoint2DLineRenderer : MonoBehaviour
{

	SpringJoint2D _springJoint2D;
	LineRenderer _lineRenderer;
	
	Vector3[] _positions = new Vector3[2];

	// Use this for initialization
	void Awake ()
	{
		_springJoint2D = GetComponent<SpringJoint2D>();
		_lineRenderer = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!_springJoint2D) return;
		if (!_springJoint2D.connectedBody) return;
		_positions[0] = (Vector3)_springJoint2D.anchor + transform.position;
		_positions[1] = _springJoint2D.connectedBody.transform.position;
		_lineRenderer.SetPositions(_positions);
	}
}
