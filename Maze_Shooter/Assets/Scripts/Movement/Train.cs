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
    Vector3 memorizedFrontPosition;

    // Update is called once per frame
    void Update()
    {
        if (trainElements.Count < 1) return;
        
        // The first train element follows this
        trainElements[0].Follow(transform, radius);

        memorizedFrontPosition = trainElements[0].transform.position;
        
        for (int i = 1; i < trainElements.Count; i++)
        {
            var nextTrainElement = trainElements[i - 1];
            trainElements[i].Follow(nextTrainElement.transform, nextTrainElement.radius);
        }
    }

    /// <summary>
    /// Removes the front-most element from the train, and returns its game object.
    /// </summary>
    public GameObject TakeFrontElement()
    {
        if (trainElements.Count < 1) return null;
        GameObject frontMost = trainElements[0].gameObject;
        trainElements.RemoveAt(0);
        frontMost.GetComponent<TrainElement>().ExitTrain();
        return frontMost;
    }

    public void PlaceInFront(GameObject element)
    {
        TrainElement t = element.GetComponent<TrainElement>();
        if (!t)
        {
            Debug.Log("Element doesn't have a train component!", element);
            return;
        }

        t.EnterTrain();
        element.transform.position = FrontPosition();
        trainElements.Insert(0, t);
    }

    /// <summary>
    /// Returns the correct position for the front element of the train.
    /// </summary>
    Vector3 FrontPosition()
    {
        if (trainElements.Count > 0)
            return trainElements[0].transform.position + Vector3.left * .01f;

        return memorizedFrontPosition;
    }
}