using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class IsoSortGroup : MonoBehaviour
{
    public List<IsoSorter> isoSorters = new List<IsoSorter>();

    public bool overrideOffset;
    
    [ShowIf("overrideOffset"), OnValueChanged("UpdateOffset")]
    public float offset;

    [Button]
    void GetSorters()
    {
        isoSorters.Clear();
        isoSorters.AddRange(GetComponentsInChildren<IsoSorter>());
    }

    void UpdateOffset()
    {
        foreach (var isoSorter in isoSorters)
        {
            isoSorter.offset = offset;
        }
    }
}
