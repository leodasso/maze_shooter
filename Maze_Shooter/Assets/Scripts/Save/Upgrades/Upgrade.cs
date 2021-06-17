using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Upgrade<T> : SavedProperty<bool>
{
	T valueToUpgrade;
	T upgradeAmount;
}
