using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[ExecuteAlways]
public class PathTrain : MonoBehaviour
{
	public AnimationCurve scaleCurve = AnimationCurve.Constant(0, 1, 1);
	public List<PathFollower> trainElements = new List<PathFollower>();
	public PathFollower master;

	[ButtonGroup]
	void GetPathFollowers() {
		trainElements.Clear();
		trainElements.AddRange(GetComponentsInChildren<PathFollower>());
		OrderTrainElements();
	}

	[ButtonGroup]
	void OrderTrainElements() 
	{
		trainElements = trainElements.OrderBy(x => x.pathPosition).ToList();
	}

	[ButtonGroup]
	void ApplyScale() {

		for (int i = 0; i < trainElements.Count; i++) {
			float progress = (float)i / (float)trainElements.Count;

			PathFollower p = trainElements[i];
			float scale = scaleCurve.Evaluate(progress);
			p.transform.localScale = Vector3.one * scale;
		}
	}


    void LateUpdate()
    {
        if (!master) return;
		// clamp master to fit within all the elements of the path
		master.pathPosition = Mathf.Clamp(master.pathPosition, MinPos(), MaxPos());

		int masterIndex = trainElements.IndexOf(master);

		// update all elements before master
		UpdateAtIndex(master, masterIndex, -1);

		// update all elements after master
		UpdateAtIndex(master, masterIndex, 1);
    }

	// Recursively update each part of the train
	void UpdateAtIndex(PathFollower prevSegment, int index, int direction = -1) 
	{
		index += direction;
		if (index < 0 || index >= trainElements.Count) return;
		var thisSegment = trainElements[index];
		float totalDist = prevSegment.FinalRadius + thisSegment.FinalRadius;
		thisSegment.pathPosition = prevSegment.pathPosition + totalDist * direction;

		UpdateAtIndex(thisSegment, index, direction);
	}

	float MinPos() 
	{
		float length = 0;
		for (int i = 0; i < trainElements.IndexOf(master); i++) 
			length += trainElements[i].FinalRadius * 2;

		return length;
	}

	float MaxPos() 
	{
		float length = 0;
		for (int i = trainElements.Count - 1; i > trainElements.IndexOf(master); i--) 
			length += trainElements[i].FinalRadius * 2;

		return master.path.PathLength - length;
	}

	
}
