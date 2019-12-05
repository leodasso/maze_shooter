using UnityEngine;

/// <summary>
/// A part of a train. A Train component can have many TrainElements following it,
/// and they will organize into a nice little path, like, yknow, train cars.
/// </summary>
[ExecuteAlways]
public class TrainElement : MonoBehaviour
{
    [Tooltip("Determines how much space is left before and after this element.")]
    public float radius = 1;

    // The ray cast from the leader toward this
    Ray _leaderRay;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void Follow(Transform leader, float leaderRadius)
    {
        _leaderRay = new Ray(leader.position, transform.position - leader.position);
        float followDist = radius + leaderRadius;
        transform.position = _leaderRay.GetPoint(followDist);
    }
}