using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arachnid;
using TMPro;

public class CandlesGui : MonoBehaviour
{
	public FloatValue candles;

	public TextMeshProUGUI wholeCandlesCount;
	public SpriteAnimator candleFractions;

	[SerializeField]
	CanvasGroupHelper fractionCandleGroup;

    // Start is called before the first frame update
    void Start()
    {
        
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
	}
}
