using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDebug : MonoBehaviour
{
	void OnCollisionEnter(Collision other) {
		foreach(var contact in other.contacts) {
			Debug.Log(name + " has a contact: " + contact.otherCollider.name + " at " + contact.point);
		}
	}
}
