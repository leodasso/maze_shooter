using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arachnid;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Ghost/Upgrades/Wallet")]
public class WalletUpgrade : ScriptableObject
{
	public int newMaxValue = 500;
	public IntValue maxMoneyValue;
 
	[Button]
	public void DoUpgrade()
	{
		int max = Mathf.Max(maxMoneyValue.Value, newMaxValue);
		Debug.Log("Upgrading wallet to the value of " + max);
		maxMoneyValue.Value = max;
	}
}
