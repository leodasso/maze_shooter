using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Matchable : MonoBehaviour
{
	[MinValue(0)]
	[Tooltip("The level of this object must correlate to it's place in the MatchableInfo list!")]
	public int level = 0;

	public MatchableInfo matchableInfo;

	// The object I've been matched with
	Matchable matchedObject = null;

    // Start is called before the first frame update
    void Start() 
    {
        
    }

	void Update() {
		if (matchedObject) {
			// Animate the objects moving towards each other
			LerpTo(matchedObject.transform.position);
			matchedObject.LerpTo(transform.position);

			// Merge them when they're close enough
			if (Vector3.Distance(transform.position, matchedObject.transform.position) < .3f)
				MergeWith(matchedObject);
		}
	}

	void OnCollisionEnter(Collision other) {
		Collider col = other.contacts[0].thisCollider;
		ProcessIntersection(col);
	}

	void OnTriggerEnter(Collider other) {
		ProcessIntersection(other);
	}

	void ProcessIntersection(Collider other) {

		if (AmMaxLevel()) return;
		var otherMatchable = other.GetComponent<Matchable>();

		if (otherMatchable == this) return;
		if (!otherMatchable) return;
		if (otherMatchable.matchableInfo != matchableInfo) return;
		if (otherMatchable.level != level) return;
		MatchWith(otherMatchable);
	}

	void SetFreeMovement(bool enabled) {
		Collider col = GetComponent<Collider>();
		col.isTrigger = enabled;
	}

	void LerpTo(Vector3 pos) {
		transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * 3);
	}

	void MatchWith(Matchable other) {

		Debug.Log("Matching with " + other.name);

		matchedObject = other;

		SetFreeMovement(true);
		other.SetFreeMovement(true);
	}

	void MergeWith(Matchable other) {

		if (AmMaxLevel()) return;

		Debug.Log("Merging with " + other.name);

		// get the next level prefab
		int nextLevel = level + 1;
		GameObject nextLevelPrefab = matchableInfo.evolutions[nextLevel];

		// instantiate the next level prefab
		Instantiate(nextLevelPrefab, transform.position, transform.rotation);

		// destroy 
		Destroy(other.gameObject);
		Destroy(gameObject);
	}

	bool AmMaxLevel() {
		if (matchableInfo == null) return true;
		return level >= matchableInfo.evolutions.Count-1;
	}
}
