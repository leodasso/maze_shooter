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

    // Start is called before the first frame update
    void Start()
    {
        
    }


	public void Recalculate()
	{
		// Whole candles get shown by the text
		wholeCandlesCount.text = Mathf.FloorToInt(candles.Value).ToString();

		// fractions of candles get shown as a sprite
		candleFractions.progress = candles.Value % 1;
	}
}
