using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[ExecuteInEditMode]
public class IsoSorter : MonoBehaviour
{

	public float offset;
	[ToggleLeft]
	public bool isStatic;
	[ReadOnly]
	public int sortingOrder;
	float _worldSpaceToSortRatio = 100;
	
	public List<SortedRenderer> renderers = new List<SortedRenderer>();

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawCube(new Vector3(transform.position.x, transform.position.y + offset, transform.position.z), Vector3.one * .1f );
	}

	// Use this for initialization
	void Start () {
		
	}

	[Button]
	void GetRenderers()
	{
		renderers.Clear();
		foreach (var r in GetComponentsInChildren<Renderer>())
		{
			SortedRenderer newRenderer = new SortedRenderer {renderer = r};
			renderers.Add(newRenderer);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (isStatic && Application.isPlaying) return;

		sortingOrder = Mathf.RoundToInt((-transform.position.y - offset) * _worldSpaceToSortRatio);

		for (int i = 0; i < renderers.Count; i++)
		{
			var r = renderers[i];
			if (r.renderer == null) continue;
			r.renderer.sortingOrder = sortingOrder + r.offset;
		}
	}

	[System.Serializable]
	public struct SortedRenderer
	{
		[HorizontalGroup(Title = "Renderer / Offset"), HideLabel]
		public Renderer renderer;
		[HorizontalGroup(), HideLabel]
		public int offset;
	}
}
