using System;
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
    [BoxGroup("saveFile"), ToggleLeft] 
    public bool debug;
    
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
    
    [BoxGroup("saveFile"), ToggleLeft, Space] 
    [Tooltip("Save the loaded value. For example, if cache is enabled and Load() is called 50 times in 1 frame, it will only" +
             " load from file once, and then save the value.")]
    public bool useCache;

    [BoxGroup("saveFile"), ShowIf("useCache")]
    [Tooltip("How long before the cache expires? If the value is loaded at 3 seconds in, and the lifetime is 1 second, " +
             "any additional calls after 4 seconds will re-load the value.")]
    public float cacheLifetime = 1;
    
    [BoxGroup("saveFile"), ShowIf("useCache"), ShowInInspector, ReadOnly]
    T _cachedValue;

    DateTime _lastLoadTime;
    
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
        #if UNITY_EDITOR
        if (debug)
        {
            Debug.Log(name + " is being saved as value: " + value);
            if (isOverride)
                Debug.LogWarning(name + " has an editor override set! If loaded, it will still return the override value, " +
                                 "even though the save file value has been updated.");
        }
        #endif
        GameMaster.SaveToCurrentFile(Prefix + name, value, this);
    }
    
    T Load()
    {
#if UNITY_EDITOR
        if (debug)
        {
            Debug.Log(name + "'s value is being loaded.");
        }
#endif
        if (CacheIsValid())
        {
            if (debug)
                Debug.Log("Cache value " + _cachedValue + " is still valid.");
            return _cachedValue;
        }

        // This block is just for giving logs in debug mode
        if (useCache && debug)
            Debug.Log("Cached value is expired; Reloading the cache.");

        _lastLoadTime = DateTime.Now;
        var saveFileValue = GameMaster.LoadFromCurrentFile(Prefix + name, defaultValue, this);
        _cachedValue = saveFileValue;
        return saveFileValue;
    }

    bool CacheIsValid()
    {
        if (!useCache) return false;
        if (debug)
            Debug.Log("Prev load time: " + _lastLoadTime + "; current time: " + DateTime.Now);

        var difference = DateTime.Now - _lastLoadTime;
        return difference.TotalSeconds < cacheLifetime;
    }
}
