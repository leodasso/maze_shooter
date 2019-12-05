using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls a train of objects moving behind this game object. Trailing objects
/// don't have to be children, but they must be referenced in the train element list,
/// and they must have TrainElement components
/// </summary>
[ExecuteAlways]
public class Train : MonoBehaviour
{
    public List<TrainElement> trainElements = new List<TrainElement>();
    public float radius = 1;

    // Update is called once per frame
    void Update()
    {
        if (trainElements.Count < 1) return;
        
        // The first train element follows this
        trainElements[0].Follow(transform, radius);
        
        for (int i = 1; i < trainElements.Count; i++)
        {
            var nextTrainElement = trainElements[i - 1];
            trainElements[i].Follow(nextTrainElement.transform, nextTrainElement.radius);
        }
    }
}