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
		[Range(0, 1)]
		public float progress;
		public FloatReference speed;
		public Vector2 offset;
		public Vector2 moveInput;

		public PathNode beginningNode;

		[ReadOnly]
		public PathNode startNode;
		[ReadOnly]
		public PathNode endNode;
		[ReadOnly]
		public PathNode prevNode;
		
		[ToggleLeft, Toggle("Adopt the sorting order of the pathnodes as it traverses the path")]
		public bool controlSortingOrder;
		
		[ShowIf("controlSortingOrder")]
		public List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

		public int sortingOrder;

		Player _player;
		NodeChoice _pendingChoice;
		
		// Use this for initialization
		void Start()
		{
			_player = ReInput.players.GetPlayer(0);
			if (beginningNode) PlaceAtNode(beginningNode);
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
			
			if (!startNode || !endNode) return;
			if (progress < 1)
			{
				float dist = Vector3.Distance(startNode.transform.position, endNode.transform.position);
				progress += Time.deltaTime * speed.Value / dist;
				transform.position = Vector3.Lerp(startNode.transform.position, endNode.transform.position, progress) + (Vector3)offset;
			}
			else
				NodeReached(endNode);
		}

		void NodeReached(PathNode node)
		{
			// If there's less than 3 node connections, then a choice isn't necessary; just continue along the path
			if (node.connectedNodes.Count < 3)
			{
				foreach (var n in node.connectedNodes)
				{
					if (n == prevNode) continue;
					BeginMovement(node, n);
					return;
				}
			}
			
			// we've gotten to a place where the player needs to make a choice
			PlaceAtNode(node);
		}

		public void PlaceAtNode(PathNode node)
		{
			transform.position = node.transform.position + (Vector3) offset;
			startNode = prevNode = node;
			_pendingChoice = new NodeChoice();
			_pendingChoice.standingNode = node;
			_pendingChoice.nodes.AddRange(node.connectedNodes);
		}

		[Button]
		void BeginMovement(PathNode newStartNode, PathNode newEndNode)
		{
			startNode = newStartNode;
			prevNode = startNode;
			endNode = newEndNode;
			progress = 0;
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