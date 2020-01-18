using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Ghost/haunt Gui Settings")]
public class HauntGuiSettings : ScriptableObject
{
    public List<HauntGuiPair> hauntGui = new List<HauntGuiPair>();

    public HauntGuiPair GetLargestHauntGui(int qty)
    {
        if (hauntGui.Count < 1)
        {
            Debug.LogError("No Haunt Gui prefabs are listed in " + name, this);
            return null;
        }

        HauntGuiPair largestFit = null;
        foreach (HauntGuiPair prefab in hauntGui)
        {
            if (prefab.value <= qty)
            {
                if (largestFit == null)
                    largestFit = prefab;
                
                else if (prefab.value > largestFit.value)
                    largestFit = prefab;
            }
        }

        if (largestFit == null)
        {
            Debug.LogWarning("No haunt gui prefabs found to fit the value " + qty, this);
            return hauntGui[0];
        }

        return largestFit;
    }
}

[System.Serializable]
public class HauntGuiPair
{
    [HorizontalGroup(), HideLabel]
    public GameObject prefab;
    [HorizontalGroup(), HideLabel]
    public int value = 1;
}