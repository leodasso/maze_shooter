using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class Chest : MonoBehaviour
{
    public UnityEvent onOpen;
    public List<ChestItem> chestItems = new List<ChestItem>();
    
    // Start is called before the first frame update
    void Start()
    {
        // if saved as opened, snap to open
        
        // place items in chest
        PlaceItemsInChest();
    }

    /// <summary>
    /// Makes the items in chest effectively not interactible while still being visible.
    /// </summary>
    void PlaceItemsInChest()
    {
        foreach (var chestItem in chestItems) 
            chestItem.onInitInChest.Invoke();
    }

    public void TakeItemsFromChest()
    {
        foreach (var chestItem in chestItems) 
            chestItem.onExitChest.Invoke();
    }

    [Button]
    void GetChestItems()
    {
        chestItems.Clear();
        chestItems.AddRange(GetComponentsInChildren<ChestItem>());
    }

    [Button]
    public void Open()
    {
        // if already opened, ignore
        
        onOpen.Invoke();
    }

    /// <summary>
    /// When the scene loads, if this chest has already been opened,
    /// we'll just snap to open for continuity sake
    /// </summary>
    void SnapToOpen()
    {
        
    }
}
