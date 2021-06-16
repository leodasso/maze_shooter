using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arachnid;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Ghost/Upgrades/Wallet")]
public class WalletUpgrade : ScriptableObject
{
	public int newMaxValue = 500;
	public SavedInt maxMoneySavedValue;
 
	[Button]
	public void DoUpgrade()
	{
		int max = Mathf.Max(maxMoneySavedValue.GetValue(), newMaxValue);
		Debug.Log("Upgrading wallet to the value of " + max);
		maxMoneySavedValue.Save(max);
	}
}
