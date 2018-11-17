using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[ExecuteInEditMode, RequireComponent(typeof(Renderer))]
public class TakeSortingOfParent : MonoBehaviour
{
	public int offset;
	SpriteRenderer _parentSpriteRenderer;
	readonly List<Renderer> _myRenderers = new List<Renderer>();

	[Button]
	void Refresh()
	{
		_myRenderers.Clear();
		_myRenderers.AddRange(GetComponents<Renderer>());
		_parentSpriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
		Execute();
	}
	
	// Use this for initialization
	void Start ()
	{
		Refresh();
	}

	void Update()
	{
		Execute();
	}

	void Execute()
	{
		if ( !_parentSpriteRenderer) return;
		foreach (var r in _myRenderers)
		{
			r.sortingLayerID = _parentSpriteRenderer.sortingLayerID;
			r.sortingOrder = _parentSpriteRenderer.sortingOrder + offset;
		}
	}
}