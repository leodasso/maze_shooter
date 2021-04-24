using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Arachnid;


[ExecuteInEditMode]
public class SpriteShapeCollider : MonoBehaviour
{
	public float height = 5;
	public float thickness = 1;
	public UnityEngine.U2D.SpriteShapeController spriteShapeController;

	[SerializeField]
	GameObject[] colliders = new GameObject[999];
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

	[ButtonGroup]
	void BuildCollider() 
	{
		// destroy any previous colliders
		RemoveCollider();

		int points = spriteShapeController.spline.GetPointCount();
		Debug.Log("Found " + points + " points on sprite shape " + name);

		for (int i = 1; i < points; i++) {
			Vector3 pt1 = spriteShapeController.spline.GetPosition(i-1);
			Vector3 pt2 = spriteShapeController.spline.GetPosition(i);
			BuildColliderSegment(pt1, pt2, i);
		}

		// build the very last collider from last point to the first point
		Vector3 last = spriteShapeController.spline.GetPosition(points-1);
		Vector3 first = spriteShapeController.spline.GetPosition(0);
		BuildColliderSegment(last, first, points);
	}

	[ButtonGroup]
	void RemoveCollider()
	{
		for (int i = 0; i < colliders.Length; i++) {
			if (colliders[i] != null) 
				DestroyImmediate(colliders[i]);
		}
	}

	void BuildColliderSegment(Vector3 pt1, Vector3 pt2, int index) 
	{
		// calculate the angle to rotate the collider to
		Vector3 offset = pt2 - pt1;
		float angle = Math.AngleFromVector2(offset, -90);

		GameObject newCol = new GameObject("col " + index);
		newCol.transform.parent = transform;
		colliders[index] = newCol;

		var newBox = newCol.AddComponent<BoxCollider>();

		float segmentLength = Vector3.Distance(pt1, pt2);
		newBox.size = new Vector3(segmentLength, height, thickness);
		newBox.center = new Vector3(segmentLength / 2, height / 2, thickness/2);
		newBox.transform.localPosition = pt1;
		newBox.transform.localEulerAngles = new Vector3(angle, -90, 90);
	}
}