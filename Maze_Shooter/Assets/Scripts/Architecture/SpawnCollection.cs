using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Ghost/Spawn Collection")]
public class SpawnCollection : ScriptableObject
{
    public List<SpawnCollectionItem> collection = new List<SpawnCollectionItem>();

    public GameObject GetRandom()
    {
        float roll = Random.Range(0, GetWeightSum());
        foreach (var item in collection)
        {
            roll -= item.chance;
            if (roll <= 0) return item.gameObject;
        }
        return null;
    }

    float GetWeightSum()
    {
        float weightSum = 0;
        foreach (var item in collection)
        {
            weightSum += item.chance;
        }
        return weightSum;
    }

    [System.Serializable]
    public class SpawnCollectionItem
    {
        [HorizontalGroup(), HideLabel, PreviewField]
        public GameObject gameObject;
        
        [Range(0, 1), HorizontalGroup(), HideLabel]
        public float chance = 1;
    }
}