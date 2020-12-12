using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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

	[BoxGroup("stars")]
	[Tooltip("Delay between spawning each star (the star itself, not the slot)")]
	public float delayBetweenStars = .1f;

	[BoxGroup("stars")]
	public float starAnimDuration = 1.5f;

	[Tooltip("'start', 'pass', and 'fail' events will be sent to the playmaker fsm.")]
	public PlayMakerFSM playMaker;

	[Button]
	public void PlayFullSequence() 
	{
		playMaker.SendEvent("start");
	}

	[Button]
	void GetHauntStarSlots() {
		hauntStarSlots.Clear();
		hauntStarSlots.AddRange(GetComponentsInChildren<HauntStarSlot>());
	}

	public void AcceptHaunt() 
	{
		if (hauntable) 
			hauntable.AcceptHaunt();

		Destroy(gameObject);
	}

	public void RejectHaunt() 
	{
		if (hauntable) 
			hauntable.RejectHaunt();

		Destroy(gameObject);
	}

	[ButtonGroup]
	void ShowSlots() 
	{
		StartCoroutine(ShowSlotsSequence());
	}

	[ButtonGroup]
	void SpawnStars() 
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

	[ButtonGroup]
	void CheckIfSlotsFilled() 
	{
		foreach(var slot in hauntStarSlots) {

			if (!slot.CheckIfFilled()) {
				playMaker.SendEvent("fail");
				return;
			}
		}

		playMaker.SendEvent("pass");
		AcceptHaunt();
	}

	[ButtonGroup]
	void RecallSlots() 
	{
		foreach (var starSlot in hauntStarSlots) starSlot.Recall();
	}

	IEnumerator SpawnStarsSequence(List<GameObject> prefabs) 
	{
		int starIndex = 0;

		while (starIndex < prefabs.Count) {
			yield return new WaitForSecondsRealtime(delayBetweenSlots);
			HauntStar newStar = 
				Instantiate( prefabs[starIndex], transform.position, Quaternion.identity, transform)
				.GetComponent<HauntStar>();

			newStar.GotoSlot(hauntStarSlots[starIndex], starAnimDuration);
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
