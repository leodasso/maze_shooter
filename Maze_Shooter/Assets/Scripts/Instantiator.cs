using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Instantiator : MonoBehaviour
{
	[ToggleLeft]
	public bool instantiateOnStart;
	
	[ToggleLeft]
	public bool instantiateStagePlayer;

	[ShowIf("instantiateStagePlayer")]
	public Stage stage;
	
	[AssetsOnly, HideIf("instantiateStagePlayer")]
	public GameObject prefabToInstantiate;



	GameObject ToInstantiate
	{
		get
		{
			if (instantiateStagePlayer) return stage.PlayerShip;
			return prefabToInstantiate;
		}
	}

	// Use this for initialization
	void Start ()
	{
		if (instantiateOnStart) Instantiate();
	}

	public void Instantiate()
	{
		if (ToInstantiate == null)
		{
			Debug.LogWarning(name + " has no referenced object to instantiate!", gameObject);
			return;
		}

		Instantiate(ToInstantiate, transform.position, transform.rotation);
	}
}
