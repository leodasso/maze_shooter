using UnityEngine;
using Sirenix.OdinInspector;
using ShootyGhost;

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

		if (!enabled) return;
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

		if (matchedObject) return;
		Debug.Log("Matching with " + other.name);

		matchedObject = other;
		// disable the matched object so it doesn't also run match behavior
		matchedObject.enabled = false;

		SetFreeMovement(true);
		other.SetFreeMovement(true);
	}

	void MergeWith(Matchable other) {

		if (AmMaxLevel()) return;

		// get the next level prefab
		int evolution = level + 1;
		GameObject evolutionPrefab = matchableInfo.evolutions[evolution];

		// instantiate the next level prefab
		GameObject evolutionInstance = Instantiate(evolutionPrefab, transform.position, transform.rotation);

		// Check if the haunter should be migrated to the next level evolution. Check both matchers for a haunter
		Haunter haunter = GetHaunter();
		if (!haunter) haunter = other.GetHaunter();
		if (haunter) {
			Hauntable evolutionHauntable = evolutionInstance.GetComponent<Hauntable>();
			if (evolutionHauntable) 
				haunter.MigrateHaunt(evolutionHauntable);
			
			else haunter.EndHaunt(evolutionInstance);
		}
		
		// destroy 
		Destroy(other.gameObject);
		Destroy(gameObject);
	}

	Haunter GetHaunter() {
		Hauntable myHauntable = GetComponent<Hauntable>();
		if (!myHauntable) return null;
		return myHauntable.haunter;
	}

	bool AmMaxLevel() {
		if (matchableInfo == null) return true;
		return level >= matchableInfo.evolutions.Count-1;
	}
}
