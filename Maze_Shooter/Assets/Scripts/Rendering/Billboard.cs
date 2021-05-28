using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Billboard : MonoBehaviour
{
	[SerializeField]
	bool updateDuringGameplay;
    Transform _camera;

	void OnDrawGizmos() 
	{
		UpdateAngle();
	}
    
    // Start is called before the first frame update
    void Start()
    {
		if (!Camera.main) return;
        _camera = Camera.main.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!_camera || !updateDuringGameplay) return;
        UpdateAngle();
    }   

	void UpdateAngle()
	{
		if (!_camera) return;
        transform.eulerAngles = new Vector3(_camera.eulerAngles.x, _camera.eulerAngles.y, transform.eulerAngles.z);
	}
}
