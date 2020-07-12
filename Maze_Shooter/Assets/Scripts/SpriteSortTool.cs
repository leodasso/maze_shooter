using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Sirenix.OdinInspector;

public class SpriteSortTool : MonoBehaviour
{
    public List<SpriteRenderer> sprites = new List<SpriteRenderer>();

    [Button]
    public void GetAllSceneSprites() 
    {
        sprites.Clear();
        sprites.AddRange(FindObjectsOfType<SpriteRenderer>());
    }

    [Button]
    public void SetDefaultSortingOrder() 
    {
        Debug.Log("Setting 0 sort order for " + sprites.Count + " sprite renderers.");

        foreach(SpriteRenderer sr in sprites) {
            if (sr.GetComponentInParent<SortingGroup>()) {
                Debug.Log(sr.name + " was skipped because it's part of a sorting group.");
                continue; 
            }
            sr.sortingOrder = 0;
        }
    }
}
