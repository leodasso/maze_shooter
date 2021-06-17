using System.Collections;
using UnityEngine;
using Arachnid;

public class CandleGulper : PickupGulper
{
	[SerializeField]
    SavedFloat savedCandles;
    
	[SerializeField]
	FloatValue candles;

	[SerializeField]
	FloatValue maxCandles;
    
    void Awake()
    {
		if (!ConfirmValueExistence(candles)) return;
		LoadSavedValue(candles, savedCandles);
    }

    void OnDestroy()
    {
        TrySave(candles, savedCandles);
    }

	protected override void OnTriggerEnter(Collider other)
	{
		TryGulpPickup<CandlePickup>(other);
	}

	protected override void OnGulp<T>(T pickup)
	{
		CandlePickup candle = pickup as CandlePickup;
		candles.Value += candle.value;
		candles.Value = Mathf.Clamp(candles.Value, 0, maxCandles.Value);
	}

	protected override bool IsFull()
	{
		return candles.Value >= maxCandles.Value;
	}
}