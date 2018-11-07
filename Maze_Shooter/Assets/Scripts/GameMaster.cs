using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameMaster : ScriptableObject
{
    public GameObject defaultPlayerShip;
    public float defaultRespawnTime = 3;
    public Stage currentStage;

    static GameMaster _gameMaster;

    public static GameMaster Get()
    {
        if (_gameMaster) return _gameMaster;
        _gameMaster = Resources.Load<GameMaster>("game master");
        return _gameMaster;
    }
}
