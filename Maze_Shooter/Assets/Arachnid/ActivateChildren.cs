using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using Sirenix.OdinInspector;

[InfoBox("Has functions to activate children in a sequence with a delay between them.")]
public class ActivateChildren : MonoBehaviour
{
    public float delayBetweenActivations = .15f;

    [ToggleLeft]
    public bool activateOnEnable = false;

    [ToggleLeft, Tooltip("Markers can be placed to give notice that something is about to spawn here, to make it " +
                         "fair to players. ")]
    public bool placePreSpawnMarkers = false;

    [ShowIf("placePreSpawnMarkers")]
    [AssetsOnly, Tooltip("The marker to show that something is about to spawn. This will be placed wherever children are")]
    public GameObject preSpawnMarker;

    [ShowIf("placePreSpawnMarkers")]
    [Tooltip("How long will pre-spawn markers be shown before the actual objects instantiate?")]
    public FloatReference preSpawnDuration;

    void OnEnable()
    {
        if (activateOnEnable) ActivateMyChildren();
    }

    public void ActivateMyChildrenImmediately()
    {
        foreach (Transform t in transform)
        {
            // Only add children that aren't already active
            if (t.gameObject.activeSelf) continue;
            t.gameObject.SetActive(true);
        }
    }

    [ButtonGroup()]
    public void ActivateMyChildren()
    {
        if (!Application.isPlaying)
        {
            // If in editor mode, just activate the dang ol children.
            foreach (Transform t in transform)
                t.gameObject.SetActive(true);
        }
        else
        {
            StartCoroutine(DoActivation());
        }
    }

    [ButtonGroup()]
    public void DeactivateMyChildren()
    {
        foreach (Transform t in transform)
            t.gameObject.SetActive(false);
    }

    IEnumerator DoActivation()
    {
        List<GameObject> childrenToBeActivated = new List<GameObject>();
        foreach (Transform t in transform)
        {
            // Only add children that aren't already active
            if (t.gameObject.activeSelf) continue;
            
            childrenToBeActivated.Add(t.gameObject);
        }

        foreach (GameObject go in childrenToBeActivated)
        {
            ActivateChild(go);
            yield return new WaitForSeconds(delayBetweenActivations);
        }
    }

    void ActivateChild(GameObject child)
    {
        if (!placePreSpawnMarkers)
            child.SetActive(true);

        else 
            StartCoroutine(SpawnSequence(child));
    }

    IEnumerator SpawnSequence(GameObject child)
    {
        // create the pre-spawn marker at the right place
        GameObject newPreSpawnMarker = Instantiate(preSpawnMarker, child.transform.position, Quaternion.identity);
        Destroy(newPreSpawnMarker, preSpawnDuration.Value);
        
        yield return new WaitForSeconds(preSpawnDuration.Value);
        child.SetActive(true);
    }
}
