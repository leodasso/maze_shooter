using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Paths
{
	[ExecuteInEditMode]
	public class PathFollower : MonoBehaviour
	{
		public Pathway pathway;
		[Range(0, 1)]
		public float progress;
		public Vector2 offset;
		[ToggleLeft, Toggle("Adopt the sorting order of the pathnodes as it traverses the path")]
		public bool controlSortingOrder;
		
		[ShowIf("controlSortingOrder")]
		public List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

		public int sortingOrder;
		
		// Use this for initialization
		void Start()
		{
			
		}

		void Update()
		{
			if (!pathway) return;
			transform.position = pathway.Position(progress) + (Vector3)offset;

			if (controlSortingOrder)
			{
				foreach (var s in spriteRenderers)
				{
					sortingOrder = pathway.SortingOrder(progress);
					s.sortingLayerName = pathway.SortingLayer(progress);
					s.sortingOrder = sortingOrder;
				}
			}
		}
	}
}