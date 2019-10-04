using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

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
	
	[ToggleLeft]
	public bool instantiateStagePlayer;

	[ToggleLeft, Tooltip("Allow for instantiation of multiple instances of the prefab?")]
	public bool allowMultipleInstances;

	[ToggleLeft, Tooltip("Apply my scale to the new instance")]
	public bool applyScale;

	[ShowIf("instantiateStagePlayer")]
	public Stage stage;
	
	[AssetsOnly, HideIf("instantiateStagePlayer"), PreviewField]
	public GameObject prefabToInstantiate;
	
	[Tooltip("Define when to create an instance of the selected prefab"), EnumToggleButtons, Title("Create Instance", bold:false, horizontalLine:false), HideLabel]
	public InstantiateBehavior createInstance = InstantiateBehavior.None;
	[Space, Tooltip("Define when to destroy the created instance."), EnumToggleButtons, Title("Destroy Instance", bold:false, horizontalLine:false), HideLabel]
	public InstantiateBehavior destroyInstance = InstantiateBehavior.None;

	[EnumToggleButtons, HideLabel, Title("Instance Place In Hierarchy", bold:false, horizontalLine:false)]
	public HierarchyType instancePlaceInHierarchy = HierarchyType.World;

	[ShowInInspector, ReadOnly]
	GameObject _instance;

	GameObject ToInstantiate
	{
		get
		{
			if (instantiateStagePlayer) return stage.PlayerShip;
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
		RunEvents(InstantiateBehavior.OnDisable);
	}

	void OnDestroy()
	{
		RunEvents(InstantiateBehavior.OnDestroy);
	}

	void RunEvents(InstantiateBehavior behavior)
	{
		if (createInstance == behavior) Instantiate();
		if (destroyInstance == behavior) RemoveInstance();
	}

	// left public to be accesible from Unity Events
	public void Instantiate()
	{
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

		_instance = Instantiate(ToInstantiate, transform.position, transform.rotation, parent);
		if (applyScale) _instance.transform.localScale = transform.localScale;
	}

	// left public to be accesible from Unity Events
	public void RemoveInstance()
	{
		if (!_instance) return;
		Destroy(_instance);
	}
}