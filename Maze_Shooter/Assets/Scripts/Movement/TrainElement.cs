using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

/// <summary>
/// A part of a train. A Train component can have many TrainElements following it,
/// and they will organize into a nice little path, like, yknow, train cars.
/// </summary>
[ExecuteAlways]
public class TrainElement : MonoBehaviour
{
    [SerializeField, ShowInInspector, TabGroup("Events")]
    UnityEvent _onEnterTrain;
    
    [SerializeField, ShowInInspector, TabGroup("Events")]
    UnityEvent _onExitTrain;
    
    [Tooltip("Determines how much space is left before and after this element.")]
    [TabGroup("Main")]
    public float radius = 1;

    // The ray cast from the leader toward this
    Ray _leaderRay;

    void Start()
    {
    }

    /// <summary>
    /// Calls a unity event for entering train so each prefab can do custom functions when it re-enters train.
    /// </summary>
    public void EnterTrain()
    {
        _onEnterTrain.Invoke();
    }

    /// <summary>
    /// Calls a unity event for leaving train so each prefab can do custom functions when it exits train.
    /// </summary>
    public void ExitTrain()
    {
        _onExitTrain.Invoke();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void Follow(Transform leader, float leaderRadius)
    {
        if (!enabled) return;
        _leaderRay = new Ray(leader.position, transform.position - leader.position);
        float followDist = radius + leaderRadius;
        transform.position = Vector3.Lerp(transform.position, _leaderRay.GetPoint(followDist), Time.deltaTime * 15);
    }
}