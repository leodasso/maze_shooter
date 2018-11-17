using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Paths
{
	[ExecuteInEditMode]
	public class Pathway : MonoBehaviour
	{
		public List<PathNode> pathNodes = new List<PathNode>();
		public int NodeCount => pathNodes.Count;

		public float Length()
		{
			float l = 0;
			for (int i = 0; i < pathNodes.Count - 1; i++)
				l += SegmentLength(i);

			return l;
		}

		float SegmentLength(int segmentIndex)
		{
			if (segmentIndex < 0 || segmentIndex >= pathNodes.Count - 1)
			{
				Debug.LogError("Segment index is out of range.", gameObject);
				return 1;
			}
			
			return Vector3.Distance(pathNodes[segmentIndex].transform.position, pathNodes[segmentIndex + 1].transform.position);
		}

		public string SortingLayer(float progress)
		{
			int index = Mathf.RoundToInt(progress * NodeCount - 1);
			index = Mathf.Clamp(index, 0, NodeCount - 1);
			return pathNodes[index].SortingLayer();
		}

		public int SortingOrder(float progress)
		{
			float distFromStart = progress * Length();

			for (int i = 0; i < pathNodes.Count - 1; i++)
			{
				float segmentLength = SegmentLength(i);
				if (distFromStart < segmentLength)
				{
					float segmentProgress = distFromStart / segmentLength;
					float order = Mathf.Lerp(pathNodes[i].SortingOrder(), pathNodes[i + 1].SortingOrder(), segmentProgress);
					return Mathf.RoundToInt(order);
				}

				distFromStart -= segmentLength;
			}
			return pathNodes[pathNodes.Count - 1].SortingOrder();
			
		}
		
		public Vector3 Position(float progress)
		{
			float distFromStart = progress * Length();

			for (int i = 0; i < pathNodes.Count - 1; i++)
			{
				float segmentLength = SegmentLength(i);
				if (distFromStart < segmentLength)
				{
					float segmentProgress = distFromStart / segmentLength;
					return Vector3.Lerp(pathNodes[i].transform.position, pathNodes[i + 1].transform.position, segmentProgress);
				}

				distFromStart -= segmentLength;
			}
			return pathNodes[pathNodes.Count - 1].transform.position;
		}

		void OnDrawGizmos()
		{
			Gizmos.color = Color.yellow;
			pathNodes = pathNodes.Where(x => x != null).ToList();
			for (int i = 0; i < pathNodes.Count - 1; i++)
			{
				Gizmos.DrawLine(pathNodes[i].transform.position, pathNodes[i + 1].transform.position);
			}
		}

		[Button]
		public void RefreshPathNodes()
		{
			pathNodes.Clear();
			pathNodes.AddRange(GetComponentsInChildren<PathNode>());
			foreach (var n in pathNodes)
				n.GetSpriteRenderer();
		}
	}
}