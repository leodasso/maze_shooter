using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Controls a train of objects moving behind this game object. Trailing objects
/// don't have to be children, but they must be referenced in the train element list,
/// and they must have TrainElement components
/// </summary>
[ExecuteAlways]
public class Train : MonoBehaviour
{
    [Tooltip("Should I control the scale of the elements of this train?")]
    public bool scaleElements;
    
    [ShowIf("scaleElements")]
    [Tooltip("Scale of the followers of the train. X axis is position in the train, 0 being front, then 1, 2, 3, ...n. " +
             "and Y axis is the scale.")]
    public AnimationCurve scaleCurve = AnimationCurve.Constant(0, 5, 1);
    
    public List<TrainElement> trainElements = new List<TrainElement>();
    public float radius = 1;
    Vector3 memorizedFrontPosition;

    public bool EmptyTrain => trainElements.Count < 1;

    void Start()
    {
        foreach (var trainElement in trainElements)
        {
            trainElement.EnterTrain();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (EmptyTrain) return;
        
        if (scaleElements) ScaleElementsUpdate();
        
        // The first train element follows this object, while the rest 
        // just follow the next train element in line
        trainElements[0].Follow(transform, radius, 0);
        
        for (int i = 1; i < trainElements.Count; i++)
        {
            var nextTrainElement = trainElements[i - 1];
            trainElements[i].Follow(nextTrainElement.transform, nextTrainElement.FinalRadius, i);
        }
        
        memorizedFrontPosition = trainElements[0].transform.position - transform.position;
    }

    /// <summary>
    /// Has each element scale to fit the scale curve of the train.
    /// </summary>
    void ScaleElementsUpdate()
    {
        for (int i = 0; i < trainElements.Count; i++)
        {
            var element = trainElements[i];
            if (!element) continue;
            
            element.SetScale(scaleCurve.Evaluate(i));
        }
    }

    /// <summary>
    /// Removes the front-most element from the train, and returns its game object.
    /// </summary>
    public GameObject TakeFrontElement()
    {
        if (EmptyTrain) return null;
        GameObject frontMost = trainElements[0].gameObject;
        trainElements.RemoveAt(0);
        frontMost.GetComponent<TrainElement>().ExitTrain();
        return frontMost;
    }

    public void PlaceInFront(GameObject element)
    {
        TrainElement t = GetTrainElement(element);
        t.EnterTrain();
        t.transform.position = FrontPosition();
        trainElements.Insert(0, t);
    }
    
    public void PlaceInBack(GameObject element)
    {
        TrainElement t = GetTrainElement(element);
        t.EnterTrain();
        trainElements.Add(t);
    }

    TrainElement GetTrainElement(GameObject go)
    {
        TrainElement t = go.GetComponent<TrainElement>();
        if (!t)
        {
            Debug.Log("Object doesn't have a train component!", go);
            return null;
        }

        return t;
    }

    /// <summary>
    /// Returns the correct position for the front element of the train.
    /// </summary>
    Vector3 FrontPosition()
    {
        if (trainElements.Count > 0)
            return trainElements[0].transform.position + Vector3.left * .01f;

        return memorizedFrontPosition + transform.position;
    }

    
    Vector3 BackPosition()
    {
        if (trainElements.Count > 0)
            return trainElements[trainElements.Count - 1].transform.position + Vector3.left * .01f;

        return memorizedFrontPosition + transform.position;
    }
}