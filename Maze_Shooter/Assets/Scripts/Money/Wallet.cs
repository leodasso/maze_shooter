using UnityEngine;
using Sirenix.OdinInspector;

public class Wallet : MonoBehaviour
{
    public SavedInt savedMoneyValue;
    
    [ShowInInspector, ReadOnly]
    int _currentMoney;

    // Failsafe to prevent from saving to file when no value loaded
    bool _moneyValueLoaded;
    
    void Awake()
    {
        // Get saved money value
        _currentMoney = savedMoneyValue.GetValue();
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
        _currentMoney += otherCoin.value;
        otherCoin.Grab();
    }

    void TrySave()
    {
        if (!_moneyValueLoaded) return;
        savedMoneyValue.Save(_currentMoney);
    }
}
