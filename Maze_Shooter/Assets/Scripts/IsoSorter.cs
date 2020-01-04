using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[ExecuteInEditMode]
public class IsoSorter : MonoBehaviour
{

	[ToggleLeft, Tooltip("Use a transform other than this one to determine sorting")]
	public bool useCustomTransform;

	[ShowIf("useCustomTransform")]
	public Transform customTransform;
	
	public float offset;
	[ToggleLeft]
	public bool isStatic;
	[ReadOnly]
	public int sortingOrder;
	float _worldSpaceToSortRatio = 100;
	
	public List<SortedRenderer> renderers = new List<SortedRenderer>();

	Transform SortingTransform => useCustomTransform && customTransform ? customTransform : transform;

	void OnDrawGizmosSelected()
	{
		if (SortingTransform == null) return;
		Gizmos.color = Color.yellow;
		Gizmos.DrawCube(new Vector3(SortingTransform.position.x, SortingTransform.position.y + offset, SortingTransform.position.z), Vector3.one * .1f );
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

		
		sortingOrder = Mathf.RoundToInt((-SortingTransform.position.z - offset) * _worldSpaceToSortRatio);

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
