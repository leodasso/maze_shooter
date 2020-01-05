using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Arachnid;
using UnityEngine.Events;

public class StarCheck : MonoBehaviour
{
    [ValidateInput("ValidateStars", "Required stars count must match the sum of stars in gate lights. Either adjust the count or " +
                                    "add/remove gate lights.", ContinuousValidationCheck = true)]
    public int requiredStars = 5;
    public List<StarSlot> starSlots = new List<StarSlot>();
    
    [Tooltip("The saved bool value that says if this star check has already passed.")]
    public SavedBool starCheckPassedStatus;

    public UnityEvent onCheckPassed;
    public UnityEvent onCheckFailed;

    GameObject _player;
    
    public Collection player;

    
    [AssetsOnly, Tooltip("The prop constellation that gets pulled out of the ghost and put into the gate lights")]
    public GameObject propConstellationPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    [Button]
    void GetGateLights()
    {
        starSlots.Clear();
        starSlots.AddRange(GetComponentsInChildren<StarSlot>());
    }
    
    bool ValidateStars(int inputStars)
    {
        int sum = 0;
        foreach (StarSlot starSlot in starSlots)
        {
            // For gate lights that are already active, no stars are required.
            if (starSlot.lightActive) continue;
            sum += starSlot.stars;
        }

        return sum == inputStars;
    }
    
    public void CheckStars()
    {
        // Get the player game object
        _player = player.GetElement(0).gameObject;
        
        // TODO Has this check already been activated?

        foreach (StarSlot starSlot in starSlots)
        {
            starSlot.TryActivate();
        }
        
        // TODO delay, like in a coroutine or something

        if (PlayerTotalStars() >= requiredStars)
            PassCheck();
        else 
            FailCheck();
    }

    void PassCheck()
    {
        // TODO saving
        onCheckPassed.Invoke();
    }

    void FailCheck()
    {
        onCheckFailed.Invoke();
    }
    
    int PlayerTotalStars()
    {
        return 10;
    }
}
