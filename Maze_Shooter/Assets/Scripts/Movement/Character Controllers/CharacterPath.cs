using System.Collections;
using System.Collections.Generic;
using Arachnid;
using Sirenix.OdinInspector;
using UnityEngine;

[TypeInfoBox("Characters can move along this path. Points can store events to send to playmaker as well. " +
	"\n-Overlap points to merge them \n-Click + button to add new points")]
public class CharacterPath : MonoBehaviour
{
	[ToggleLeft, Tooltip("Does the path connect start to end point?")]
	public bool looped;

	[Tooltip("Drag points close together to merge them.")]
	public List<CharacterPathPoint> pathPoints = new List<CharacterPathPoint>();


	public void InsertNewPoint(int index, Vector3 worldPos)
	{
		CharacterPathPoint newPoint = new CharacterPathPoint();
		newPoint.pos = transform.InverseTransformPoint(worldPos);
		pathPoints.Insert(index, newPoint);
	}

	[ButtonGroup]
	void CenterOnPathPoints() 
	{
		// Calculate center of all points
		Vector3 center = Vector3.zero;
		foreach(var p in pathPoints) {
			center += p.pos;
		}
		center /= pathPoints.Count;

		// move all points to offset
		foreach(var p in pathPoints) {
			p.pos -= center;
		}

		transform.Translate(center);
	}

	[ButtonGroup]
	void FlattenHeight()
	{
		foreach (var p in pathPoints) {
			p.pos = new Vector3(p.pos.x, 0, p.pos.z);
		}
	}

	[Button]
	void GenerateDefaultPath()
	{
		for (int i = 0; i < pathPoints.Count; i++) 
			pathPoints[i] = null;

		pathPoints.Clear();

		float radius = 3;
		CharacterPathPoint p1 = new CharacterPathPoint(new Vector3(-radius, 0, 0));
		CharacterPathPoint p2 = new CharacterPathPoint(new Vector3(0, 0, radius));
		CharacterPathPoint p3 = new CharacterPathPoint(new Vector3(radius, 0, 0));
		CharacterPathPoint p4 = new CharacterPathPoint(new Vector3(0, 0, -radius));

		pathPoints.Add(p1);
		pathPoints.Add(p2);
		pathPoints.Add(p3);
		pathPoints.Add(p4);
	}


	[Button]
	void ReversePath()
	{
		pathPoints.Reverse();
	}


	public void RemovePoint(int index) 
	{
		pathPoints[index] = null;
		pathPoints.RemoveAt(index);
	}



	[System.Serializable]
	public class CharacterPathPoint 
	{
		public Vector3 pos;
		public CharacterPathPoint() {}

		public CharacterPathPoint(Vector3 newPos) {
			pos = newPos;
		}
	}
}