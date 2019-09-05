using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EllipseMovement : MonoBehaviour
{

	public float radius = 1;
	public float speed = 5;

	float _progress = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{

		_progress += Time.deltaTime * speed;

		float x = Mathf.Sin(_progress) * radius;
		float y = Mathf.Cos(_progress) * radius;
		transform.localPosition = new Vector3(x, y);
	}
}
