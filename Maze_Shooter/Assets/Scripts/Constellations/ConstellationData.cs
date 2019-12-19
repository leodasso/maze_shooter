using UnityEngine;
using Sirenix.OdinInspector;
public enum ConstellationStatus
{
    FromSaveFile,
    OverrideAsCollected,
    OverrideAsNotCollected,
}

[CreateAssetMenu(menuName ="Ghost/Constellation Data")]
public class ConstellationData : ScriptableObject
{
    public ConstellationStatus constellationStatus = ConstellationStatus.FromSaveFile;
    public string title = "Please name me";

    const string SaveKeyPrefix = "constellation_";

    public bool HasBeenCollected()
    {
#if UNITY_EDITOR 
        if (constellationStatus == ConstellationStatus.OverrideAsCollected) 
            return true;
        if (constellationStatus == ConstellationStatus.OverrideAsNotCollected) 
            return false;
#endif
        return CollectedInSaveFile();
    }

    /// <summary>
    /// Returns the saved status of this constellation, whether it's been collected or not.
    /// </summary>
    bool CollectedInSaveFile()
    {
        return GameMaster.LoadFromCurrentFile(SaveKeyPrefix + name, false, this);
    }

    [ButtonGroup()]
    public void SaveAsCollected()
    {
        SaveStatus(true);
    }
    
    [ButtonGroup()]
    void SaveAsNotCollected()
    {
        SaveStatus(false);
    }

    void SaveStatus(bool isCollected)
    {
        GameMaster.SaveToCurrentFile(SaveKeyPrefix + name, isCollected, this);
    }

    [Button]
    void LogSaveStatus()
    {
        Debug.Log(name + " is collected: " + CollectedInSaveFile());
    }


}
