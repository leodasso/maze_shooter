using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arachnid;

public class Energy : MonoBehaviour
{
	public FloatValue energy;
	public FloatReference maxEnergy;
	public FloatReference energyRechargeRate;

    // Start is called before the first frame update
    void Start()
    {
        energy.Value = maxEnergy.Value;
    }

	void Update()
	{
		if (energy.Value < maxEnergy.Value)
			energy.Value += energyRechargeRate.Value * Time.deltaTime;
	}

	public bool ConsumeEnergy(float amount) 
	{
		if (energy.Value < amount) return false;

		energy.Value -= amount;
		return true;
	}
}
