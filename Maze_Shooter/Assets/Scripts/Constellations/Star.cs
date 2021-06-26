using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Sirenix.OdinInspector;

public class Star : MonoBehaviour
{
	[FormerlySerializedAs("myConstellation")]
    public StarData starData;
    public GameObject galaxyPrefab;
    [Tooltip("If the star has already been collected in the past, this is what will show")]
    public GameObject collectedVisuals;
    [Tooltip("If the star is new, this is what will show")]
    public GameObject notCollectedVisuals;
    public InteractivePanel titleGuiPrefab;
    public Transform galaxySpawnPoint;

    public UnityEvent onNewlyCollected;
    public UnityEvent onReCollected;
    public UnityEvent onAddToGalaxy;

    bool _touched;
    
    // Start is called before the first frame update
    void Start()
    {
        IndicateIfCollected(starData.HasBeenCollected());
    }

    /// <summary>
    /// This affects the visuals only - sets the star to indicate that it has already been collected
    /// </summary>
    [ButtonGroup()]
    void ShowAsCollected()
    {
        IndicateIfCollected(true);
    }

    /// <summary>
    /// This affects the visuals only - sets the star to indicated that it's new!
    /// </summary>
    [ButtonGroup()]
    void ShowAsNew()
    {
        IndicateIfCollected(false);
    }

    void IndicateIfCollected(bool hasBeenCollected)
    {
        collectedVisuals.SetActive(hasBeenCollected);
        notCollectedVisuals.SetActive(!hasBeenCollected);
    }

    public void OnTouched()
    {
        // Ensure there's no double activations
        if (_touched) return;
        _touched = true;

        // Remember if it had been collected before saving the new value
        bool beenCollected = starData.HasBeenCollected();
        
        starData.Value = true;
        
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
        newGalaxy.starToFocus = starData;
        newGalaxy.StarInstance = this;
        newGalaxy.showConstellationAcquire.Invoke();
    }

    public void ShowTitleGui()
    {
        var titleBox = titleGuiPrefab.CreateInstance() as StarTitle;
        titleBox.starData = starData;
        titleBox.ShowPanel();
    }
}
