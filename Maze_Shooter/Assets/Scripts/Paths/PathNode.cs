using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace Paths
{
	[ExecuteInEditMode]
	public class PathNode : MonoBehaviour
	{
		public List<PathNode> connectedNodes = new List<PathNode>();
		[ToggleLeft]
		public bool available;
		[ToggleLeft] 
		public bool requiresChoice;

		public StageCrystal linkedCrystal;
		public UnityEvent onNodeReached;
		public UnityEvent onNodeLeft;

		SpriteRenderer _spriteRenderer;

		[Button]
		void FixConnections()
		{
			foreach (var n in connectedNodes)
			{
				if (n == null) continue;
				if (n.connectedNodes.Contains(this)) continue;
				n.connectedNodes.Add(this);
				EditorTools.SetDirty(n);
			}
		}

		void OnDrawGizmos()
		{
			Gizmos.color = Color.yellow;
			foreach (var n in connectedNodes)
			{
				if (n == null) continue;
				Gizmos.DrawLine(transform.position, n.transform.position);
			}
		}

		// Use this for initialization
		void Start()
		{
			_spriteRenderer = GetComponent<SpriteRenderer>();
			
			if (Application.isPlaying && _spriteRenderer) 
				_spriteRenderer.enabled = false;
		}

		[Button]
		public void GetSpriteRenderer()
		{
			_spriteRenderer = GetComponent<SpriteRenderer>();
		}

		public string SortingLayer()
		{
			return _spriteRenderer == null ? "Default" : _spriteRenderer.sortingLayerName;
		}

		public int SortingOrder()
		{
			return _spriteRenderer == null ? 0 : _spriteRenderer.sortingOrder;
		}
	}
}