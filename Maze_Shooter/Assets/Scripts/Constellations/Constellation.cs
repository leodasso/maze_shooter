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
        
        // TODO logic to see if this is collected in the save file
        onNewlyCollected.Invoke();
    }

    public void OpenGalaxy()
    {
        Galaxy newGalaxy = Instantiate(galaxyPrefab, galaxySpawnPoint.position, quaternion.identity).GetComponent<Galaxy>();
        newGalaxy.constellationToFocus = myConstellation;
        newGalaxy.showConstellationAcquire.Invoke();
    }

    public void ShowTitleGui()
    {
        var titleBox = titleGuiPrefab.CreateInstance() as ConstellationTitle;
        titleBox.constellationData = myConstellation;
        titleBox.ShowPanel();
    }
}
