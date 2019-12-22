using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName ="Ghost/Constellation Data")]
[TypeInfoBox("Handles info and saving for constellations. The bool value here is 'has been collected'")]
public class ConstellationData : SavedBool
{
    public string title = "Please name me";
    protected override string Prefix => "constellation_";

    public bool HasBeenCollected()
    {
        return GetValue();
    }
}
