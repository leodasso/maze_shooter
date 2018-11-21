using System.Collections;
using System.Collections.Generic;
using Arachnid;
using Rewired;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Paths
{
	public class PathFollower : MonoBehaviour
	{

		public FloatReference speed;
		public Vector2 offset;
		public Vector2 moveInput;

		public PathNode beginningNode;
		
		[Toggle("Adopt the sorting order of the pathnodes as it traverses the path"), ToggleGroup("controlSortingOrder")]
		public bool controlSortingOrder;
		
		[ToggleGroup("controlSortingOrder")]
		public List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

		[ToggleGroup("controlSortingOrder")]
		public int sortingOrder;

		[ReadOnly, HideLabel, HorizontalGroup("traversal/nodes", Title = "Start & End Nodes"), ShowInInspector]
		PathNode _startNode;
		[ReadOnly, HideLabel, HorizontalGroup("traversal/nodes"), ShowInInspector]
		PathNode _endNode;
		[Range(0, 1), BoxGroup("traversal"), ShowInInspector]
		float _progress;

		Rewired.Player _player;
		NodeChoice _pendingChoice;
		PathNode _prevNode;
		
		// Use this for initialization
		void Start()
		{
			_player = ReInput.players.GetPlayer(0);
			if (beginningNode) SetInitialNode(beginningNode);
		}

		void SetInitialNode(PathNode node)
		{
			PlaceAtNode(beginningNode);
			node.onNodeReached.Invoke();
			node.linkedCrystal?.SetSelected(true);
		}

		void Update()
		{
			if (_pendingChoice != null)
			{
				moveInput = new Vector2(_player.GetAxis("moveX"), _player.GetAxis("moveY"));
				if (moveInput.magnitude > .2f)
					SelectPathway();
				
				return;
			}
			
			if (!_startNode || !_endNode) return;
			if (_progress < 1)
			{
				float dist = Vector3.Distance(_startNode.transform.position, _endNode.transform.position);
				_progress += Time.deltaTime * speed.Value / dist;
				transform.position = Vector3.Lerp(_startNode.transform.position, _endNode.transform.position, _progress) + (Vector3)offset;
			}
			else
				NodeReached(_endNode);
			
			if (controlSortingOrder) SetSorting(_progress);
		}

		void SetSorting(float progress)
		{
			if (!_startNode || !_endNode) return;
			string sortingLayer = progress < .5f ? _startNode.SortingLayer() : _endNode.SortingLayer();
			int order = Mathf.RoundToInt(Mathf.Lerp(_startNode.SortingOrder(), _endNode.SortingOrder(), progress));
			ApplySorting(sortingLayer, order);
		}

		void ApplySorting(string sortingLayer, int order)
		{
			sortingOrder = order;
			foreach (var r in spriteRenderers)
			{
				r.sortingOrder = order;
				r.sortingLayerName = sortingLayer;
			}
		}

		void NodeReached(PathNode node)
		{
			node.onNodeReached.Invoke();
			node.linkedCrystal?.SetSelected(true);
			
			if (node.requiresChoice)
			{
				PlaceAtNode(node);
				return;
			}
			
			// If there's less than 3 node connections, then a choice isn't necessary; just continue along the path
			if (node.connectedNodes.Count < 3)
			{
				foreach (var n in node.connectedNodes)
				{
					if (n == _prevNode) continue;
					BeginMovement(node, n);
					return;
				}
			}
			
			// we've gotten to a place where the player needs to make a choice
			PlaceAtNode(node);
		}

		void PlaceAtNode(PathNode node)
		{
			transform.position = node.transform.position + (Vector3) offset;
			_startNode = _prevNode = node;
			_pendingChoice = new NodeChoice();
			_pendingChoice.standingNode = node;
			_pendingChoice.nodes.AddRange(node.connectedNodes);
			ApplySorting(node.SortingLayer(), node.SortingOrder());
		}

		void BeginMovement(PathNode newStartNode, PathNode newEndNode)
		{
			newStartNode.onNodeLeft.Invoke();
			newStartNode.linkedCrystal?.SetSelected(false);
			_startNode = newStartNode;
			_prevNode = _startNode;
			_endNode = newEndNode;
			_progress = 0.05f;
			_pendingChoice = null;
		}

		/// <summary>
		/// Chooses the pathway that best represents the player's input
		/// </summary>
		void SelectPathway()
		{
			if (_pendingChoice == null) return;

			Vector2 normalizedInput = moveInput.normalized;
			PathNode nearestNode = null;
			float closestDot = -99;

			foreach (var n in _pendingChoice.nodes)
			{
				if (!n.available) continue;
				Vector2 dir = n.transform.position - _pendingChoice.standingNode.transform.position;
				float dot = Vector2.Dot(normalizedInput, dir.normalized);
				if (dot > closestDot)
				{
					nearestNode = n;
					closestDot = dot;
				}
			}
			if (closestDot < 0) return;
			
			BeginMovement(_pendingChoice.standingNode, nearestNode);
		}
		
		class NodeChoice
		{
			public PathNode standingNode;
			public List<PathNode> nodes = new List<PathNode>();
		}
	}
}