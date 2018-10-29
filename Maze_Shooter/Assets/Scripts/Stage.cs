using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Stage")]
public class Stage : ScriptableObject
{
    public IntReference lives;
    public bool useDefaultPlayerShip = true;
    [HideIf("useDefaultPlayerShip")]
    public GameObject customShip;

    public FloatReference startingDelay;
}
