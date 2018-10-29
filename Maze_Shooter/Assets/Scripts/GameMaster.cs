using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameMaster : ScriptableObject
{
    public GameObject defaultPlayerShip;
    public float defaultRespawnTime = 3;
}
