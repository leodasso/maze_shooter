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
    [TabGroup("Main"), ShowInInspector, SerializeField]
    float radius = 1;
    
    [Tooltip("Does the local scale of this object affect the follow radius? (uses x scale).")]
    [TabGroup("Main")]
    public bool scaleAffectsRadius = true;

    [TabGroup("Main")] 
    public float followSpeed = 15;
    
    [Tooltip("Optional - if an Animator is added, this component will send inTrain and trainIndex info to the animator.")]
    [TabGroup("Main")]
    public Animator animator;

    // The ray cast from the leader toward this
    Ray _leaderRay;

    Vector3 _initScale;
    float _scale;
    bool _lerpScale = false;

    public float FinalRadius => scaleAffectsRadius ? transform.localScale.x * radius : radius;

    void Start()
    {
        _initScale = transform.localScale;
    }

    void Update()
    {
        if (_lerpScale)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, _initScale * _scale, Time.deltaTime * 8);
        }
    }

    /// <summary>
    /// Calls a unity event for entering train so each prefab can do custom functions when it re-enters train.
    /// </summary>
    public void EnterTrain()
    {
        _onEnterTrain.Invoke();
        if (animator)
            animator.SetBool("inTrain", true);
    }

    /// <summary>
    /// Calls a unity event for leaving train so each prefab can do custom functions when it exits train.
    /// </summary>
    public void ExitTrain()
    {
        _onExitTrain.Invoke();
        transform.localScale = _initScale;
        _lerpScale = false;
        if (animator)
           animator.SetBool("inTrain", false);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 0, .3f);
        Gizmos.DrawWireSphere(transform.position, FinalRadius);
    }

    public void SetScale(float scale)
    {
        _lerpScale = true;
        _scale = scale;
    }

    public void Follow(Transform leader, float leaderRadius, int index)
    {
        if (!enabled) return;
        _leaderRay = new Ray(leader.position, transform.position - leader.position);
        float followDist = FinalRadius + leaderRadius;
        transform.position = Vector3.Lerp(transform.position, _leaderRay.GetPoint(followDist), Time.deltaTime * followSpeed);
        
        if (Application.isPlaying && animator)
            animator.SetInteger("trainIndex", index);
    }
    
    
}