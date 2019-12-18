using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class Constellation : MonoBehaviour
{
    public ConstellationData myConstellation;
    public GameObject galaxyPrefab;
    public InteractivePanel titleGuiPrefab;
    public Transform galaxySpawnPoint;

    public UnityEvent onNewlyCollected;
    public UnityEvent onReCollected;
    public UnityEvent onAddToGalaxy;

    bool _touched;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnTouched()
    {
        // Ensure there's no double activations
        if (_touched) return;
        _touched = true;

        bool beenCollected = myConstellation.HasBeenCollected();
        
        myConstellation.SaveAsCollected();
        
        // Different behaviors depending on if it's already been collected.
        // Constellations that have been collected before can still appear in the scene, but we don't want to do
        // the whole song and dance when you get it again. Maybe just a quick bit of feedback to show that it's 
        // been touched, but has already been collected.
        if (beenCollected)
            onReCollected.Invoke();
        else
            onNewlyCollected.Invoke();
    }

    public void OpenGalaxy()
    {
        Galaxy newGalaxy = Instantiate(galaxyPrefab, galaxySpawnPoint.position, quaternion.identity).GetComponent<Galaxy>();
        newGalaxy.constellationToFocus = myConstellation;
        newGalaxy.constellationInstance = this;
        newGalaxy.showConstellationAcquire.Invoke();
    }

    public void ShowTitleGui()
    {
        var titleBox = titleGuiPrefab.CreateInstance() as ConstellationTitle;
        titleBox.constellationData = myConstellation;
        titleBox.ShowPanel();
    }
}
