using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arachnid;
using TMPro;
using Sirenix.OdinInspector;

public class CandlesGui : MonoBehaviour
{
	public FloatValue candles;
	public IntValue burningCandles;

	[Title("Qty Text")]
	[SerializeField, Space]
	TextMeshProUGUI wholeCandlesCount;
	[SerializeField]
	TextMeshProUGUI burningCandlesCount;

	[Title("Candle Fractions")]
	[SerializeField, Space]
	SpriteAnimator candleFractions;
	[SerializeField]
	SpriteAnimator candleBurningFractions;

	[SerializeField, Space]
	CanvasGroupHelper fractionCandleGroup;

	[SerializeField]
	CanvasGroupHelper normalFractionCandle;

	[SerializeField]
	CanvasGroupHelper burningFractionCandle;

	[Title("Burning Candles")]
	[SerializeField, Space]
	CanvasGroupHelper burningCandlesGroup;
	[SerializeField]
	CanvasGroupHelper smallQtyGroup;
	[SerializeField]
	CanvasGroupHelper largeQtyGroup;

	float fractionCandleProgress;

	[SerializeField]
	List<CandleBurningGui> burningCandleGuis = new List<CandleBurningGui>();

	// if the number of burning candles is bigger than this, we use just a normal counter
	const int smallQty = 10;

	bool IsBurning => burningCandles.Value > 0;

    // Start is called before the first frame update
    void Start()
    {
        burningCandlesGroup.SnapAlpha(0);
		smallQtyGroup.SnapAlpha(0);
		largeQtyGroup.SnapAlpha(0);

		Recalculate();
    }

	void Update() 
	{
		SetCandleProgress(Mathf.Lerp(candleFractions.progress, fractionCandleProgress, Time.unscaledDeltaTime * 3));
	}

	[Button]
	void GetBurningCandleGuis()
	{
		burningCandleGuis.Clear();
		burningCandleGuis.AddRange(GetComponentsInChildren<CandleBurningGui>());
	}

	public void RecalculateBurningCandles()
	{
		burningCandlesGroup.SetAlpha(IsBurning ? 1 : 0);
		
		bool useSmallQtyGroup = burningCandles.Value <= smallQty;

		// Turn on or off the individual burning candles
		if (useSmallQtyGroup) {
			for (int i = 0; i < burningCandleGuis.Count; i++) {
				bool candleIsActive = i < burningCandles.Value;
				burningCandleGuis[i].gameObject.SetActive(candleIsActive);
				burningCandleGuis[i].SetVisible(candleIsActive);
			}
		}

		// Show the individual candles or just 'candles X 15' thing based on how many there are
		smallQtyGroup.SetAlpha(useSmallQtyGroup ? 1 : 0);
		largeQtyGroup.SetAlpha(useSmallQtyGroup ? 0 : 1);

		burningCandlesCount.text = burningCandles.Value.ToString();
	}


	float fractionCandlePercent;
	float fractionCandlePercentPrev;

	public void Recalculate()
	{
		// Whole candles get shown by the text
		wholeCandlesCount.text = Mathf.FloorToInt(candles.Value).ToString();

		// show the burning candle or unlit candle
		normalFractionCandle.SetAlpha(IsBurning ? 0 : 1);
		burningFractionCandle.SetAlpha(IsBurning ? 1 : 0);

		fractionCandlePercentPrev = fractionCandlePercent;
		fractionCandlePercent = candles.Value % 1;

		// determine whether to show the fractional candle
		bool showFraction = fractionCandlePercent > .05f;
		fractionCandleGroup.SetAlpha(showFraction ? 1 : 0);

		// fractions of candles get shown as a sprite
		// if the value has gone down (meaning we've used up another candle) just snap to the full candle
		bool bigChange = Mathf.Abs(fractionCandlePercent - fractionCandlePercentPrev) > .85f;
		if (bigChange) {
			fractionCandleProgress = fractionCandlePercent;
			SetCandleProgress(fractionCandleProgress);
		}
		fractionCandleProgress = fractionCandlePercent;
	}

	void SetCandleProgress(float newProgress) 
	{
		candleFractions.progress = newProgress;
		candleBurningFractions.progress = newProgress;
	}
}