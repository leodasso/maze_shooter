using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ghost/Save/Saved Hearts")]
public class SavedHearts : SavedProperty<Hearts>
{
	protected override void Save()   
    {
        if (debug)
            Debug.Log(name + " is being saved as value: " + runtimeValue);

		// save hearts as an integer of total points
        GameMaster.SaveToCurrentFile(Prefix + name, runtimeValue.TotalPoints, this);

		if (debug) 
			LogSaveStatus();
    }
}
