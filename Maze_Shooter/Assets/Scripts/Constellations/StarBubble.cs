using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class StarBubble : MonoBehaviour
{
	public UnityEvent onUnlock;
	public GameObject constellation;

	[SerializeField]
	float gravity = -10;
	float ySpeed = 0;
	bool falling = false;

	[Button]
	public void Unlock() {
		onUnlock.Invoke();
	}

	public void StartFalling() {
		falling = true;
	}

	public void StopFalling() {
		falling = false;
	}

	void Update() {
		if (falling) {
			ySpeed += Time.unscaledDeltaTime * gravity;
			transform.Translate(Vector3.up * ySpeed);
		}else {
			ySpeed = 0;
		}
	}

	void OnDisable() {
		constellation.transform.parent = null;
	}
}
