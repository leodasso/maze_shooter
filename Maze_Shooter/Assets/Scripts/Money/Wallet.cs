using System.Collections;
using UnityEngine;
using Arachnid;

public class Wallet : MonoBehaviour
{
    public SavedInt savedMoneyValue;

	[SerializeField]
	float coinInWalletDelay = .5f;
    
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

    void OnTriggerEnter(Collider other)
    {
        Coin otherCoin = other.GetComponent<Coin>();
        if (!otherCoin) return;
		StartCoroutine(AddCoin(otherCoin.value));
        otherCoin.Grab();
    }

	IEnumerator AddCoin(int value) {
		yield return new WaitForSecondsRealtime(coinInWalletDelay);
		_money.Value += value;
	}

    void TrySave()
    {
        if (!_moneyValueLoaded) return;
        savedMoneyValue.Save(_money.Value);
    }
}
