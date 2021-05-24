using System.Collections;
using UnityEngine;
using Arachnid;

public class Wallet : PickupGulper
{
    public SavedInt savedMoneyValue;
    
	[SerializeField]
	IntValue _money;

    // Failsafe to prevent from saving to file when no value loaded
    bool _moneyValueLoaded;
    
    void Awake()
    {
        // Get saved money value
		if (_money == null) {
			Debug.LogError("Wallet has no intValue referenced to store money!", gameObject);
			enabled = false;
			return;
		}
        _money.Value = savedMoneyValue.GetValue();
        _moneyValueLoaded = true;
    }

    void OnDestroy()
    {
        // save money value
        TrySave();
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

    void TrySave()
    {
        if (!_moneyValueLoaded) return;
        savedMoneyValue.Save(_money.Value);
    }
}