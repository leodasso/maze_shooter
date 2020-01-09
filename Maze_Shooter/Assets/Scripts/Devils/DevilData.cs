using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The save data for a devil. the bool value here is RECRUITED. If the value is true,
/// it means this devil has been recruited by the player.
/// </summary>
[CreateAssetMenu(menuName = "Ghost/Devil Data")]
public class DevilData : SavedBool
{
    public string title = "Causer of Havoc";
    protected override string Prefix => "devil_";
    public GameObject prefab;

    public bool IsRecruited()
    {
        return GetValue();
    }
}
