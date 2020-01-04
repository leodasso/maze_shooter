using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;

public class CashCrystal : MonoBehaviour
{
    public int cashValue = 1;

    public void GrabCrystal()
    {
        // TODO money??
        //GameMaster.cashThisSession += cashValue;
        Destroy(gameObject);
    }
}
