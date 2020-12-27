using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Sirenix.OdinInspector;

[ExecuteAlways]
public class ObjectsAlongLine : MonoBehaviour
{
	public CinemachinePathBase path;
	[Tooltip("The prefab to place instances of along the path")]
	public GameObject prefab;

	[MinValue(1), MaxValue(999)]
	public int maxCount = 99;

	[MinValue(.01f)]
	public float spacing = .5f;

	[ShowInInspector, ReadOnly]
	public List<Transform> instances = new List<Transform>();


    // Update is called once per frame
    void Update()
    {
		if (!path) return;

		for (int i = 0; i < instances.Count; i++) 
		{
			Vector3 pos = path.EvaluatePositionAtUnit(i * spacing, CinemachinePathBase.PositionUnits.Distance);
			instances[i].transform.position = pos;
		}
    }

	[ButtonGroup]
	void UpdateInstances() 
	{
		ClearInstances();

		int count = Mathf.RoundToInt(path.PathLength / spacing);
		for (int i = 0; i < count; i++) {
			#if UNITY_EDITOR
			GameObject newInstance = UnityEditor.PrefabUtility.InstantiatePrefab(prefab, transform) as GameObject;
			instances.Add(newInstance.transform);
			#endif
		}
	}

	[ButtonGroup]
	void ClearInstances() 
	{
		foreach(var instance in instances) {
			if (Application.isPlaying)
				Destroy(instance.gameObject);
			else {
				#if UNITY_EDITOR
				GameObject.DestroyImmediate(instance.gameObject);
				#endif
			}
		}
		instances.Clear();
	}
}
