using System.Collections;
using System.Collections.Generic;
using Paths;
using UnityEngine;
using Sirenix.OdinInspector;

[TypeInfoBox("Uses attached Line Renderer to show a pathway along Path Nodes."), ExecuteInEditMode]
public class PathRenderer : MonoBehaviour
{
	[MinMaxSlider(0, 1, true)]
	public Vector2 pathCoverage = new Vector2(0, 1);
	public List<PathNode> pathNodes = new List<PathNode>();
	public LineRenderer foregroundLine;
	public LineRenderer backgroundLine;
	Vector3[] _positions;

	float _pathLength;
	List<Vector3> _pathPositions = new List<Vector3>();

	// Use this for initialization
	void Start () 
	{
		Refresh();
	}

	void Update()
	{
		Refresh();
	}


	void Refresh()
	{
		_pathLength = GetLengthOfPath();
		
		if (backgroundLine)
			SetLineRendererPositions(backgroundLine);
		
		if (foregroundLine)
			SetLineRendererPositions(foregroundLine, pathCoverage.x, pathCoverage.y);
	}

	/// <summary>
	/// Set positions of vertexes for the line renderer. 
	/// </summary>
	/// <param name="start">Where along the path to start rendering the line (normalized, 0 is beginning of path, 1 is end)</param>
	/// <param name="end">Where along the path to stop rendering the line (normalized, 0 is beginning of path, 1 is end)</param>
	void SetLineRendererPositions(LineRenderer lineRenderer, float start = 0, float end = 1)
	{
		if (end <= start) return;

		float pathProgress = 0;
		
		// Prog or progress refers to the distance from the start of the path to that point.
		float startProg = _pathLength * start;
		float endProg = _pathLength * end;
		
		_pathPositions.Clear();
		for (int i = 0; i < pathNodes.Count; i++)
		{
			// memorize this node
			PathNode node = pathNodes[i];
			
			// if this is the last node on the list, add it and break
			if (i == pathNodes.Count - 1)
			{
				_pathPositions.Add(node.transform.position);
				break;
			}
			
			// memorize the next node
			PathNode nextNode = pathNodes[i + 1];
			
			// distance from this node to the next node
			float segmentLength = Vector3.Distance(node.transform.position, nextNode.transform.position);

			// get distance from path start to this node, and to next node
			float thisNodeProg = pathProgress;
			float nextNodeProg = pathProgress + segmentLength;
			pathProgress += segmentLength;
			
			// If the starting point is beyond the next node, just continue the loop.
			if (nextNodeProg < startProg) continue;
			
			// if the starting point is between this node and the next node, find the exact starting point
			if (startProg >= thisNodeProg && startProg < nextNodeProg)
			{
				Vector3 startPos = PositionAlongPath(segmentLength, startProg, nextNodeProg, node.transform, nextNode.transform);
				_pathPositions.Add(startPos);
			}

			// if this node is beyond the starting point
			if (startProg < thisNodeProg)
			{
				_pathPositions.Add(node.transform.position);
			}

			// if the ending point is in this segment, add it and break the loop
			if (endProg >= thisNodeProg && endProg < nextNodeProg)
			{
				Vector3 endPos = PositionAlongPath(segmentLength, endProg, nextNodeProg, node.transform, nextNode.transform);
				_pathPositions.Add(endPos);
				break;
			}
		}
		
		lineRenderer.positionCount = _pathPositions.Count;
		lineRenderer.SetPositions(_pathPositions.ToArray());
	}

	/// <summary>
	/// Returns a vector3 position at t distance from the start of the path
	/// </summary>
	/// <param name="segmentLength">The length of the segment being used (a segment being the line between two adjacent nodes</param>
	/// <param name="t">A value representing distance from the start of the path</param>
	/// <param name="progressOfNextNode">The distance of the next node from the beginning of the path</param>
	Vector3 PositionAlongPath(float segmentLength, float t, float progressOfNextNode, Transform thisNodePos, Transform nextNodePos)
	{
		float alongPath = progressOfNextNode - t;
		float normalizedAlongPath = alongPath / segmentLength;

		return Vector3.Lerp(nextNodePos.position, thisNodePos.transform.position, normalizedAlongPath);
	}

	/// <summary>
	/// Returns the length of the 
	/// </summary>
	/// <param name="endingNode">(Optional) Will return the length only up until this node in the path</param>
	float GetLengthOfPath(PathNode endingNode = null)
	{
		float sum = 0;
		for (int i = 0; i < pathNodes.Count - 1; i++)
		{
			// get the length from this point to the next
			sum += Vector3.Distance(pathNodes[i].transform.position, pathNodes[i + 1].transform.position);
			if (pathNodes[i + 1] == endingNode)
				break;
		}

		return sum;
	}
}
