using System.Collections;
using UnityEngine;
using Arachnid;

public class Wallet : PickupGulper
{
	[SerializeField]
    SavedInt savedMoneyValue;
    
	[SerializeField]
	IntValue _money;

	[SerializeField, Space]
	SavedInt savedMaxMoney;
    
    void Awake()
    {
		if (!ConfirmValueExistence(_money)) return;
		LoadSavedValue(_money, savedMoneyValue);
    }

    void OnDestroy()
    {
        // save money value
        TrySave(_money, savedMoneyValue);
    }

	protected override void OnTriggerEnter(Collider other)
	{
		TryGulpPickup<Coin>(other);
	}

	protected override void OnGulp<T>(T pickup)
	{
		Coin coin = pickup as Coin;
		_money.Value += coin.value;
	}

	protected override bool IsFull()
	{
		return _money.Value >= savedMaxMoney.GetValue();
	}
}