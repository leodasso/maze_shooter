using UnityEngine;
using Sirenix.OdinInspector;
using Arachnid;

[CreateAssetMenu(menuName ="Ghost/Constellation Data")]
[TypeInfoBox("Handles info and saving for constellations. The bool value here is 'has been collected'")]
public class StarData : ValueAsset<bool>
{
    public string title = "Please name me";
    protected override string Prefix() => "constellation_";

    public bool HasBeenCollected()
    {
        return myValue;
    }

	protected override bool CanSave()
	{
		return true;
	}

	protected override bool ValueHasChanged(bool newValue)
	{
		return newValue != Value;
	}

	// intentionally blank
	protected override void ProcessValueChange(bool newValue)
	{
	}
}
