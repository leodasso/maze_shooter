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

	[BoxGroup("candle holders")]
	public float timePerCandle = .1f;

	[BoxGroup("candle holders")]
	public float slotAnimDuration = .5f;

	[BoxGroup("candle holders"), SerializeField]
	GameObject candleHolderPrefab;

	[BoxGroup("candle holders"), SerializeField]
	GameObject stringPrefab;

	[BoxGroup("candle holders"), SerializeField]
	Vector3 candleHoldersOffset = Vector3.up * 10;

	[BoxGroup("candle holders"), SerializeField]
	float spiralHeight = 1;

	[BoxGroup("candle holders"), SerializeField, Tooltip("Sets the position of each candle holder on every update frame.")]
	bool controlCandleHolderPositions;

	[BoxGroup("candle holders/spiral")]
	public float theta = .1f;

	[BoxGroup("candle holders/spiral")]
	public float thetaVelocity;

	[BoxGroup("candle holders/spiral")]
	public float thetaAcceleration;

	[BoxGroup("candle holders/spiral")]
	public float archimedes = 1;

	[BoxGroup("candle holders/spiral")]
	public int spiralStartOffset = 2;

	[BoxGroup("candle holders/spiral")]
	public float angleOffset;

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

	// number of candles the player has available. we use ceiling because 
	// fractions of a candle are still usable, they will just burn down quicker
	int candles => Mathf.CeilToInt(availableCandles.Value);


	void OnDrawGizmosSelected()
	{
		Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
		for (int i = 0; i < cost; i ++)
		{
			var color = Color.Lerp(Color.red, Color.blue, (float)i / cost);
			Gizmos.color = color;
			var pos = SlotLocalPos(i);
			Gizmos.DrawWireSphere(pos, .3f);

			if (i > 0)
				Gizmos.DrawLine(pos, SlotLocalPos(i-1));

			Gizmos.color = new Color(color.r, color.g, color.b, .3f);
			Gizmos.DrawLine(pos, Vector3.zero);
		}

		// draw line from start to end 
		Gizmos.DrawLine(SlotLocalPos(0), SlotLocalPos(cost - 1));
	}

	void Update()
	{
		thetaVelocity += thetaAcceleration * Time.unscaledDeltaTime;
		theta += thetaVelocity * Time.unscaledDeltaTime;

		if (controlCandleHolderPositions)
		{
			for (int i = 0; i < candleHolders.Count; i++)
			{
				var h = candleHolders[i];
				if (!h) continue;
				h.transform.localPosition = Vector3.Lerp(h.transform.localPosition, SlotLocalPos(i), Time.unscaledDeltaTime * 10);
			}
		}
	}


	public void Init(Hauntable newHauntable, int hauntCost)
	{
		hauntable = newHauntable;
		cost = hauntCost;

		PlayFullSequence();
	}


	/// <summary>
	/// Uses a spiral calculation to return a nice looking placement for any index
	/// </summary>
	Vector3 SlotLocalPos(int index)
	{
		Vector2 coords = Math.PointOnSpiral((float)(index + spiralStartOffset) * theta, archimedes, angleOffset);
		return Math.Project2Dto3D(coords) + candleHoldersOffset + Vector3.up * spiralHeight * index;
	}


	[Button]
	public void PlayFullSequence() 
	{		
		float timeForCandleHolders = cost * timePerCandle + .5f;
		float timeForCandles = Mathf.Min(candles, cost) * timePerCandle + .5f;

		playMaker.FsmVariables.GetFsmFloat("showCandleHoldersTime").Value = timeForCandleHolders;
		playMaker.FsmVariables.GetFsmFloat("showCandlesTime").Value = timeForCandles;

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
		controlCandleHolderPositions = true;
		foreach (var candleHolder in candleHolders) 
			candleHolder.Recall();
	}


	IEnumerator SpawnCandlesSequence() 
	{
		int candleIndex = 0;
		int candlesToSpawn = candles;

		while (candleIndex < candleHolders.Count) {

			// make sure there are candles available
			if (candlesToSpawn < 1) continue;

			
			// consume a candle and instantiate it
			candlesToSpawn--;
			HauntCandle newCandleInstance = 
				Instantiate( candlePrefab, transform.position + SlotLocalPos(candleIndex), Quaternion.identity, transform)
				.GetComponent<HauntCandle>();

			//newCandleInstance.GotoSlot(candleHolders[candleIndex], candleAnimDuration);
			candleIndex++;
			

			// do a delay between spawning each candle
			yield return new WaitForSecondsRealtime(timePerCandle);
		}
	}


	IEnumerator ShowSlotsSequence() 
	{
		int slotIndex = 0;

		while (slotIndex < cost) {

			// Instantiate candle holder
			HauntCandleHolder newHolder = Instantiate(candleHolderPrefab, transform.position + SlotLocalPos(slotIndex), Quaternion.identity)
				.GetComponent<HauntCandleHolder>();

			newHolder.transform.parent = transform;
			newHolder.PlayAnimation(0, 1, slotAnimDuration);
			candleHolders.Add(newHolder);

			// instantiate line to center
			LineRendererStraight line = Instantiate(stringPrefab, transform).GetComponent<LineRendererStraight>();
			line.startPoint = transform;
			line.endPoint = newHolder.transform;

			// instantiate line to prev candle
			if (slotIndex > 1) {
				LineRendererStraight line2 = Instantiate(stringPrefab, transform).GetComponent<LineRendererStraight>();
				line2.startPoint = candleHolders[slotIndex-1].transform;
				line2.endPoint = newHolder.transform;
			}
			slotIndex++;

			yield return new WaitForSecondsRealtime(timePerCandle);
		}
	}
}
