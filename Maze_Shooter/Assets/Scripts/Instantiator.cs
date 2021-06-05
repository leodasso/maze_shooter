using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Instantiator : MonoBehaviour
{
	public enum InstantiateBehavior
	{
		None,
		OnAwake,
		OnStart,
		OnEnable,
		OnDisable,
		OnDestroy
	}

	public enum HierarchyType
	{
		World,
		AsSibling,
		AsChild
	}

	public enum RotationType
	{
		ThisRotation,
		Prefab,
		Zero
	}
	
	[ToggleLeft]
	public bool instantiateStagePlayer;

	[ToggleLeft, Tooltip("Allow for instantiation of multiple instances of the prefab?")]
	public bool allowMultipleInstances;

	[ToggleLeft, Tooltip("Apply my scale to the new instance")]
	public bool applyScale;

	[ToggleLeft, Tooltip("Apply a random scale to the new instance")]
	public bool randomScale;

	[ShowIf("randomScale"), MinMaxSlider(0.01f, 10)]
	public Vector2 randomScaleRange = Vector2.one;
	
	[AssetsOnly, HideIf("instantiateStagePlayer"), PreviewField]
	public GameObject prefabToInstantiate;
	
	[Tooltip("Define when to create an instance of the selected prefab"), EnumToggleButtons, Title("Create Instance", bold:false, horizontalLine:false), HideLabel]
	public InstantiateBehavior createInstance = InstantiateBehavior.None;
	[Space, Tooltip("Define when to destroy the created instance."), EnumToggleButtons, Title("Destroy Instance", bold:false, horizontalLine:false), HideLabel]
	public InstantiateBehavior destroyInstance = InstantiateBehavior.None;

	[EnumToggleButtons, HideLabel, Title("Instance Place In Hierarchy", bold:false, horizontalLine:false)]
	public HierarchyType instancePlaceInHierarchy = HierarchyType.World;

	[EnumToggleButtons, HideLabel, Title("Instance Rotation", bold: false, horizontalLine: false)]
	public RotationType instanceRotation = RotationType.ThisRotation;

	[ShowInInspector, ReadOnly]
	GameObject _instance;

	GameObject ToInstantiate
	{
		get
		{
			if (instantiateStagePlayer) return GameMaster.Get().currentStage.PlayerShip;
			return prefabToInstantiate;
		}
	}

	void Awake()
	{
		RunEvents(InstantiateBehavior.OnAwake);
	}

	void Start ()
	{
		RunEvents(InstantiateBehavior.OnStart);
	}

	void OnEnable()
	{
		RunEvents(InstantiateBehavior.OnEnable);
	}

	void OnDisable()
	{
		// If this is being disabled because we're loading a scene, we don't want it to create an effect.
		if (GameMaster.transitioning) return;
		if (Time.unscaledTime < Mathf.Epsilon) return;
		RunEvents(InstantiateBehavior.OnDisable);
	}

	void OnDestroy()
	{
		// If this is being disabled because we're loading a scene, we don't want it to create an effect.
		if (GameMaster.transitioning) return;
		if (Time.unscaledTime < Mathf.Epsilon) return;
		RunEvents(InstantiateBehavior.OnDestroy);
	}

	void RunEvents(InstantiateBehavior behavior)
	{
		if (!Application.isPlaying) return;
		if (createInstance == behavior) Instantiate();
		if (destroyInstance == behavior) RemoveInstance();
	}

	// left public to be accesible from Unity Events
	public void Instantiate()
	{		
		if (!GhostTools.SafeToInstantiate(gameObject)) return;
		if (_instance != null && !allowMultipleInstances) return;
		
		if (ToInstantiate == null)
		{
			Debug.LogWarning(name + " has no referenced object to instantiate!", gameObject);
			return;
		}

		// Determine where in the hierarchy to instantiate
		Transform parent = null;
		switch (instancePlaceInHierarchy)
		{
				case HierarchyType.AsChild:
					parent = transform;
					break;
				case HierarchyType.AsSibling:
					parent = transform.parent;
					break;
		}

		if (!Application.isPlaying) return;

		Quaternion rotation = transform.rotation;
		switch (instanceRotation)
		{
				case RotationType.Prefab:
					rotation = ToInstantiate.transform.rotation;
					break;
				case RotationType.Zero:
					rotation = Quaternion.Euler(Vector3.zero);
					break;
		}
		
		_instance = Instantiate(ToInstantiate, transform.position, rotation, parent);
		if (applyScale) _instance.transform.localScale = transform.localScale;
		if (randomScale) {
			float scale = Random.Range(randomScaleRange.x, randomScaleRange.y);
			_instance.transform.localScale *= scale;
		}
	}

	// left public to be accesible from Unity Events
	public void RemoveInstance()
	{
		if (!_instance) return;
		Destroy(_instance);
	}
}