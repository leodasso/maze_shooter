using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ghost/Devil Data")]
public class DevilData : SavedBool
{
    public string title = "Causer of Havoc";
    protected override string Prefix => "devil_";
    public GameObject prefab;
}
