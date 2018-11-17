using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Paths
{
	[ExecuteInEditMode]
	public class PathNode : MonoBehaviour
	{
		[InfoBox("No pathway could be found in parents.", InfoMessageType.Warning, "NoPathway")]
		public Pathway pathway;

		bool NoPathway => pathway == null;
		SpriteRenderer _spriteRenderer;
		
		// Use this for initialization
		void Start()
		{
			if (!pathway) GetPathway();
			pathway?.RefreshPathNodes();
			_spriteRenderer = GetComponent<SpriteRenderer>();
			
			if (Application.isPlaying && _spriteRenderer) 
				_spriteRenderer.enabled = false;
		}

		[Button]
		public void GetSpriteRenderer()
		{
			_spriteRenderer = GetComponent<SpriteRenderer>();
		}

		[Button]
		void GetPathway()
		{
			pathway = GetComponentInParent<Pathway>();
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