using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using ShootyGhost;
using Arachnid;

public class HauntConstellation : MonoBehaviour
{
	[SerializeField]
	Hauntable hauntable;

	[SerializeField]
	int cost;

	[SerializeField]
	FloatValue availableCandles;

	[SerializeField]
	Vector3 candleHoldersOffset = Vector3.up * 10;

	[BoxGroup("candle holders")]
	[Tooltip("How long to wait (in seconds) between showing each haunt star slot")]
	public float delayBetweenSlots = .1f;

	[BoxGroup("candle holders")]
	public float slotAnimDuration = .5f;

	[BoxGroup("candle holders"), SerializeField]
	GameObject candleHolderPrefab;

	[BoxGroup("candles"), SerializeField]
	GameObject candlePrefab;

	[BoxGroup("candles"), SerializeField]
	GameObject flameSpiritPrefab;

	[BoxGroup("candles")]
	[Tooltip("Delay between spawning each candle")]
	public float delayBetweenCandles = .1f;

	[BoxGroup("candles")]
	public float candleAnimDuration = 1.5f;

	[Tooltip("'candles', 'quick', 'pass', and 'fail' events will be sent to the playmaker fsm.")]
	public PlayMakerFSM playMaker;

	List<HauntCandleHolder> candleHolders = new List<HauntCandleHolder>();

	void OnDrawGizmosSelected()
	{
		for (int i = 0; i < cost; i ++)
		{
			Gizmos.color = Color.Lerp(Color.red, Color.blue, (float)i / cost);
			Gizmos.DrawWireSphere(transform.TransformPoint(SlotLocalPos(i)), .3f);
		}
	}

	public void Init(Hauntable newHauntable, int hauntCost)
	{
		// TODO
		hauntable = newHauntable;
		cost = hauntCost;
	}

	Vector3 SlotLocalPos(int index)
	{
		// TODO actually nice positions
		return Vector3.right * index + candleHoldersOffset;
	}

	[Button]
	public void PlayFullSequence() 
	{
		if (cost < 1) 
			PlayQuickSequence();
		else
			playMaker.SendEvent("start");
	}

	public void PlayQuickSequence() 
	{
		playMaker.SendEvent("quick");
	}

	public void SetShortStarAnim() 
	{
		candleAnimDuration = .3f;
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
	void SpawnCandles() 
	{
		// sequence for spawning stars, matching them to slots
		if (Mathf.Ceil(availableCandles.Value) > 0)
			StartCoroutine(SpawnCandlesSequence());
	}

	[ButtonGroup]
	void CheckIfSlotsFilled() 
	{
		bool pass = true;
		foreach(var slot in candleHolders) 
			if (!slot.CheckIfFilled()) pass = false;

		if (pass)
			playMaker.SendEvent("pass");
		else
			playMaker.SendEvent("fail");
	}

	[ButtonGroup]
	void RecallSlots() 
	{
		foreach (var candleHolder in candleHolders) 
			candleHolder.Recall();
	}

	IEnumerator SpawnCandlesSequence() 
	{
		int candleIndex = 0;

		// number of candles the player has available. we use ceiling because 
		// fractions of a candle are still usable, they will just burn down quicker
		int candles = Mathf.CeilToInt(availableCandles.Value);

		while (candleIndex < candleHolders.Count) {

			// do a delay between spawning each candle
			yield return new WaitForSecondsRealtime(delayBetweenSlots);

			// make sure there are candles available
			if (candles < 1) continue;

			// consume a candle and instantiate it
			candles--;
			HauntCandle newCandleInstance = 
				Instantiate( candlePrefab, candleHolders[candleIndex].transform.position, Quaternion.identity, transform)
				.GetComponent<HauntCandle>();

			newCandleInstance.GotoSlot(candleHolders[candleIndex], candleAnimDuration);
			candleIndex++;
		}
	}

	IEnumerator ShowSlotsSequence() 
	{
		int slotIndex = 0;

		while (slotIndex < candleHolders.Count) {
			yield return new WaitForSecondsRealtime(delayBetweenSlots);
			candleHolders[slotIndex].PlayAnimation(0, 1, slotAnimDuration);
			slotIndex++;
		}
	}
}
