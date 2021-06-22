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

	[SerializeField, MinValue(0), MaxValue(500)]
	int cost;

	[SerializeField]
	FloatValue availableCandles;

	[SerializeField, Range(0,1), Tooltip("Percentage of candles / holders that must be spawned before progressing to next stage. Sends 'next' event to playmaker. ")]
	float progressThreshhold = .8f;

	[BoxGroup("candle holders"), SerializeField]
	[Tooltip("X axis is the index of the candle being spawned, and y axis is the time that candle takes to spawn. This way the first few candles can take their time, and the 100s, 200s, etc spawn very quickly")]
	AnimationCurve spawnWaitAtIndex;

	[BoxGroup("candle holders"), SerializeField]
	float slotAnimDuration = .5f;

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

	[BoxGroup("candle holders/spiral"), SerializeField]
	AnimationCurve spiralSpacing;

	[BoxGroup("candle holders/spiral"), SerializeField]
	float spacingMultiplier = 1;

	[BoxGroup("candle holders/spiral"), SerializeField]
	float spiralResolution = 500;

	[BoxGroup("candle holders/spiral"), SerializeField]
	float theta = .1f;

	[BoxGroup("candle holders/spiral")]
	public float archimedes = 1;

	[BoxGroup("candle holders/spiral")]
	public int spiralStartOffset = 2;

	[BoxGroup("candle holders/spiral"), SerializeField]
	float angleOffset;

	[BoxGroup("candles"), SerializeField]
	GameObject candlePrefab;

	[BoxGroup("candles"), SerializeField]
	GameObject flameSpiritPrefab;

	[BoxGroup("candles")]
	[Tooltip("Delay between spawning each candle")]
	public float delayBetweenCandles = .1f;

	[BoxGroup("candles")]
	public float candleAnimDuration = 1.5f;

	[Tooltip("'next', 'quick', 'pass', and 'fail' events will be sent to the playmaker fsm.")]
	public PlayMakerFSM playMaker;

	List<HauntCandleHolder> candleHolders = new List<HauntCandleHolder>();

	// number of candles the player has available. we use ceiling because 
	// fractions of a candle are still usable, they will just burn down quicker
	int candles => Mathf.CeilToInt(availableCandles.Value);

	Vector3[] spiralPts = new Vector3[1000];

	[SerializeField]
	List<Vector3> slotPts = new List<Vector3>();

	public float test;

	void OnDrawGizmosSelected()
	{
		Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);

		GenerateSpiral();
		GenerateSlotPts();

		// draw the spiral
		for (int i = 1; i < spiralPts.Length; i ++)
		{
			var color = Color.Lerp(Color.red, Color.blue, (float)i / spiralPts.Length);
			Gizmos.color = new Color(color.r, color.g, color.b, .3f);

			Gizmos.DrawLine(spiralPts[i-1], spiralPts[i]);
		}

		// draw the points on the spiral
		for (int i = 0; i < cost; i ++)
		{
			var color = Color.Lerp(Color.red, Color.blue, (float)i / cost);
			Gizmos.color = color;
			var pos = slotPts[i];
			Gizmos.DrawWireSphere(pos, .3f);
		}
	}


	public void Init(Hauntable newHauntable, int hauntCost)
	{
		hauntable = newHauntable;
		cost = Mathf.Clamp(hauntCost, 0, 500);

		PlayFullSequence();
	}

	void GenerateSpiral()
	{
		for (int i = 0; i < spiralPts.Length; i ++)
		{
			spiralPts[i] = SpiralPos(i * (1 / spiralResolution));
		}
	}


	/// <summary>
	/// Uses a spiral calculation to return a nice looking placement for any index
	/// </summary>
	Vector3 SpiralPos(float index)
	{
		Vector2 coords = Math.PointOnSpiral(spiralStartOffset + index * theta, archimedes, angleOffset);
		return Math.Project2Dto3D(coords) + candleHoldersOffset + Vector3.up * spiralHeight * index;
	}

	void GenerateSlotPts()
	{
		slotPts.Clear();
		for (int i = 0; i < 500; i ++) {
			float curvedPt = spiralSpacing.Evaluate(i) * spacingMultiplier;

			//account for the spiral's resolution
			curvedPt *= spiralResolution;
			curvedPt = Mathf.Clamp(curvedPt, 0, 999);
			slotPts.Add( spiralPts[Mathf.RoundToInt(curvedPt)] );
		}
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
		foreach(var slot in candleHolders) 
			slot.CheckIfFilled();

		if (availableCandles.Value >= cost)
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
		bool eventSent = false;

		int candlesToSpawn = Mathf.Min(candles, cost);

		for (int i = 0; i < candlesToSpawn; i++) {

			// consume a candle and instantiate it
			HauntCandle newCandleInstance = 
				Instantiate( candlePrefab, transform.position + slotPts[i], Quaternion.identity, transform)
				.GetComponent<HauntCandle>();

			newCandleInstance.GotoSlot(candleHolders[i], candleAnimDuration);			

			// do a delay between spawning each candle
			float waitTime = spawnWaitAtIndex.Evaluate(i);
			yield return new WaitForSecondsRealtime(waitTime);

			// tell the playmaker when to progress to next stage
			if (CalculateProgress(i, candlesToSpawn) > progressThreshhold && !eventSent) {
				eventSent = true;
				playMaker.SendEvent("next");
			}
		}
	}


	IEnumerator ShowSlotsSequence() 
	{
		bool eventSent = false;

		for (int i = 0; i < cost; i++) {

			// Instantiate candle holder
			HauntCandleHolder newHolder = Instantiate(candleHolderPrefab, transform.position + slotPts[i], Quaternion.identity)
				.GetComponent<HauntCandleHolder>();

			newHolder.transform.parent = transform;
			newHolder.PlayAnimation(0, 1, slotAnimDuration);
			candleHolders.Add(newHolder);

			// instantiate line to center
			LineRendererStraight line = Instantiate(stringPrefab, transform).GetComponent<LineRendererStraight>();
			line.startPoint = transform;
			line.endPoint = newHolder.transform;

			// instantiate line to prev candle
			if (i > 1) {
				LineRendererStraight line2 = Instantiate(stringPrefab, transform).GetComponent<LineRendererStraight>();
				line2.startPoint = candleHolders[i-1].transform;
				line2.endPoint = newHolder.transform;
			}

			float waitTime = spawnWaitAtIndex.Evaluate(i);
			yield return new WaitForSecondsRealtime(waitTime);

			// tell the playmaker when to progress to next stage
			float progress = CalculateProgress(i, cost);
			Debug.Log("Spawn candle holders progress: " + progress);
			if ( progress > progressThreshhold && !eventSent) {
				eventSent = true;
				playMaker.SendEvent("next");
			}
		}
	}

	float CalculateProgress (int index, float max) => (float)(index + 1) / max;
}
