using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelVolume : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

	void OnTriggerEnter(Collider other) {

		LevelObject levelObject = other.GetComponent<LevelObject>();
		if (!levelObject) return;
		levelObject.AddVolume(this);

	}

	void OnTriggerExit(Collider other) {
		LevelObject levelObject = other.GetComponent<LevelObject>();
		if (!levelObject) return;
		levelObject.RemoveVolume(this);
	}
}
