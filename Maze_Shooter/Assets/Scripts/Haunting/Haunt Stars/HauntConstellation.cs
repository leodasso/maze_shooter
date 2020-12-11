using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using ShootyGhost;

public class HauntConstellation : MonoBehaviour
{
	public Hauntable hauntable;

	[BoxGroup("star slots")]
	[Tooltip("How long to wait (in seconds) between showing each haunt star slot")]
	public float delayBetweenSlots = .1f;

	[BoxGroup("star slots")]
	public float slotAnimDuration = .5f;

	[BoxGroup("star slots")]
	public List<HauntStarSlot> hauntStarSlots = new List<HauntStarSlot>();

	[Tooltip("Delay between spawning each star (the star itself, not the slot)")]
	public float delayBetweenStars = .1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	[Button]
	void GetHauntStarSlots() {
		hauntStarSlots.Clear();
		hauntStarSlots.AddRange(GetComponentsInChildren<HauntStarSlot>());
	}

	[Button]
	public void ShowSlots() 
	{
		StartCoroutine(ShowSlotsSequence());
	}

	[Button]
	public void SpawnStars() 
	{
		List<GameObject> starPrefabs = new List<GameObject>();

		// get available stars from the current stage
		var stage = GameMaster.Get().currentStage;
		if (stage) starPrefabs.AddRange(stage.GetHauntStarPrefabs());

		// TODO get available shatterstars from the haunter
		if (hauntable) {

		}
		
		// sequence for spawning stars, matching them to slots
		StartCoroutine(SpawnStarsSequence(starPrefabs));
	}

	IEnumerator SpawnStarsSequence(List<GameObject> prefabs) 
	{
		int starIndex = 0;

		while (starIndex < prefabs.Count) {
			yield return new WaitForSecondsRealtime(delayBetweenSlots);
			GameObject newStar = Instantiate( prefabs[starIndex], transform.position, Quaternion.identity, transform);
			starIndex++;
		}
	}

	IEnumerator ShowSlotsSequence() 
	{
		int slotIndex = 0;

		while (slotIndex < hauntStarSlots.Count) {
			yield return new WaitForSecondsRealtime(delayBetweenSlots);
			hauntStarSlots[slotIndex].PlayAnimation(0, 1, slotAnimDuration);
			slotIndex++;
		}
	}
}
