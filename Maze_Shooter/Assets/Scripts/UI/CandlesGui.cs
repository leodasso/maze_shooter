using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arachnid;
using TMPro;

public class CandlesGui : MonoBehaviour
{
	public FloatValue candles;
	public IntValue burningCandles;

	[SerializeField, Space]
	TextMeshProUGUI wholeCandlesCount;
	[SerializeField]
	TextMeshProUGUI burningCandlesCount;
	[SerializeField]
	SpriteAnimator candleFractions;

	[SerializeField, Space]
	CanvasGroupHelper burningCandlesGroup;
	[SerializeField]
	CanvasGroupHelper smallQtyGroup;
	[SerializeField]
	CanvasGroupHelper largeQtyGroup;

	[SerializeField]
	CanvasGroupHelper fractionCandleGroup;

	// if the number of burning candles is bigger than this, we use just a normal counter
	const int smallQty = 10;

    // Start is called before the first frame update
    void Start()
    {
        burningCandlesGroup.SnapAlpha(0);
		smallQtyGroup.SnapAlpha(0);
		largeQtyGroup.SnapAlpha(0);
    }

	public void RecalculateBurningCandles()
	{
		burningCandlesGroup.SetAlpha(burningCandles.Value > 0 ? 1 : 0);
		
		bool useSmallQtyGroup = burningCandles.Value <= smallQty;

		smallQtyGroup.SetAlpha(useSmallQtyGroup ? 1 : 0);
		largeQtyGroup.SetAlpha(useSmallQtyGroup ? 0 : 1);

		burningCandlesCount.text = burningCandles.Value.ToString();
	}


	public void Recalculate()
	{
		// Whole candles get shown by the text
		wholeCandlesCount.text = Mathf.FloorToInt(candles.Value).ToString();

		// determine whether to show the fractional candle
		float fractionCandle = candles.Value % 1;
		bool showFraction = fractionCandle > .05f;
		fractionCandleGroup.SetAlpha(showFraction ? 1 : 0);

		// fractions of candles get shown as a sprite
		candleFractions.progress = fractionCandle;

		RecalculateBurningCandles();
	}
}
