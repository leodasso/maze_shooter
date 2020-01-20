using ShootyGhost;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.Events;

public class HauntCostGui : MonoBehaviour
{
    [ShowInInspector, ReadOnly]
    Hauntable _hauntable;
    
    [ShowInInspector, ReadOnly]
    public int _cost;

    public int fulfilledAmount;
    
    public float progressLerpSpeed = 12;

    [Tooltip("The mask is scaled up and down to change the filled amount of the progress bar(s)")]
    public Transform progressBarMask;

    public TextMeshPro numberText;

    public FollowObject followObject;

    public UnityEvent onShow;
    public UnityEvent onHide;

    float _barScale;
    float _normalizedProgress;
    Vector3 _progressBarMaskInitScale;

    public void Init(Hauntable linkedHauntable)
    {
        _hauntable = linkedHauntable;
        _cost = _hauntable.hauntCost;
        followObject.objectToFollow = linkedHauntable.gameObject;
        Recalculate();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _progressBarMaskInitScale = progressBarMask.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        Recalculate();
    }

    void Recalculate()
    {
        numberText.text = (_cost - fulfilledAmount).ToString();
        
        // Turn the amount into a number between 0 and 1, so we can feed it to the progress bars
        _normalizedProgress = Mathf.Clamp01((float)fulfilledAmount / _cost);

        _barScale = Mathf.Lerp(_barScale, _normalizedProgress, Time.unscaledDeltaTime * progressLerpSpeed);
        progressBarMask.localScale = _progressBarMaskInitScale * _barScale;
    }

    public void Show()
    {
        onShow.Invoke();
    }

    public void Hide()
    {
        onHide.Invoke();
    }
}
