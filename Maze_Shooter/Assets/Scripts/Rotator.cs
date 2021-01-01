using UnityEngine;

public class Rotator : MonoBehaviour
{
	public Space space;
	public Vector3 rotationSpeed;
	public bool unscaledTime;
	float deltaTime => unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

    void Update()
    {
        transform.Rotate(rotationSpeed * deltaTime, space);
    }
}