using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum SavedValueType
{
    FromSaveFile,
    Override,
}

/// <summary>
/// Saved properties are in-editor representations of properties in the save file.
/// They allow to organize and easily see all the properties that will be in the build's save file.
/// Saved property values can be overwritten in editor, but even if 'overwrite' is left on, it
/// will be ignored in build so we don't have to go through and make sure all the overwrites
/// are off before building.
/// </summary>
public abstract class SavedProperty<T> : ScriptableObject
{
    [BoxGroup("saveFile")]
    [Tooltip("In build, the save value is always loaded from the save file. In editor, you can " +
             "overwrite the value for testing and use the editor override below.")]
    public SavedValueType editorLoadType = SavedValueType.FromSaveFile;

    [BoxGroup("saveFile")]
    [EnableIf("isOverride")]
    public T editorOverride;
    
    [BoxGroup("saveFile")]
    [Tooltip("If the value is requested when it hasn't yet been added " +
             "to the save file, this is what will be returned.")]
    public T defaultValue;
    
    bool isOverride => editorLoadType == SavedValueType.Override;
    
    protected virtual string Prefix => "";

    public T GetValue()
    {
#if UNITY_EDITOR
        if (isOverride) return editorOverride;
#endif
        return Load();
    }


    [ButtonGroup("saveFile/btn")]
    protected virtual void SaveOverrideToFile()
    {
        Save(editorOverride);
    }

    [ButtonGroup("saveFile/btn")]
    protected virtual void LogSaveStatus()
    {
        Debug.Log(name + " value in save file: " + Load());
    }


    public virtual void Save(T value)   
    {
        GameMaster.SaveToCurrentFile(Prefix + name, value, this);
    }
    
    T Load()
    {
        return GameMaster.LoadFromCurrentFile(Prefix + name, defaultValue, this);
    }
}
