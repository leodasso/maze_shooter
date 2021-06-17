using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Saved properties are in-editor representations of properties in the save file.
/// They allow to organize and easily see all the properties that will be in the build's save file.
/// </summary>
public abstract class SavedProperty<T> : ScriptableObject
{
    [BoxGroup("saveFile"), ToggleLeft] 
    public bool debug;
    
    [BoxGroup("saveFile")]
    [Tooltip("If the value is requested when it hasn't yet been added to the save file, this is what will be returned.")]
    public T defaultValue;
    
	[BoxGroup("saveFile")]
	[Tooltip("the non-persistent value")]
	public T runtimeValue;

    protected virtual string Prefix => "";

    [ButtonGroup("saveFile/btn")]
    protected void LogSaveStatus()
    {
        Debug.Log(name + " value in save file: " + Load());
    }

	[ButtonGroup("saveFile/btn")]
	protected virtual void ResetToDefault()
	{
		runtimeValue = defaultValue;
		Save();
		LogSaveStatus();
	}


    protected virtual void Save()   
    {
        if (debug)
            Debug.Log(name + " is being saved as value: " + runtimeValue);

        GameMaster.SaveToCurrentFile(Prefix + name, runtimeValue, this);

		if (debug) 
			LogSaveStatus();
    }
    
    protected T Load()
    {
		runtimeValue = GameMaster.LoadFromCurrentFile(Prefix + name, defaultValue, this);

		if (debug)
            Debug.Log(name + "'s value was loaded as " + runtimeValue );

		return runtimeValue;
    }

    /// <summary>
    /// Checks if there's any data in the current save file for this value.
    /// </summary>
    public bool HasSavedValue()
    {
        return GameMaster.DoesKeyExist(Prefix + name);
    }
}