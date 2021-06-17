using System.Collections;
using UnityEngine;
using Arachnid;

public class Wallet : PickupGulper
{
	[SerializeField]
	IntValue _money;

	[SerializeField]
	IntValue maxMoney;
    
    void Awake()
    {
		ConfirmValueExistence(_money);
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
		return _money.Value >= maxMoney.Value;
	}
}