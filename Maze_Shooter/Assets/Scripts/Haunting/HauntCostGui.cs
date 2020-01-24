using ShootyGhost;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.Events;

public class HauntCostGui : MonoBehaviour
{
    [ShowInInspector, ReadOnly]
    Hauntable _hauntable;
    
    public float progressLerpSpeed = 12;

    [Tooltip("The mask is scaled up and down to change the filled amount of the progress bar(s)")]
    public Transform progressBarMask;

    public TextMeshPro numberText;

    public FollowObject followObject;

    public UnityEvent onShow;
    public UnityEvent onHide;
    public UnityEvent onShowFull;

    float _barScale;
    float _normalizedProgress;
    Vector3 _progressBarMaskInitScale;
    
    void Awake ()
    {
        _progressBarMaskInitScale = progressBarMask.localScale;
    }
    
    public void Init(Hauntable linkedHauntable)
    {
        _hauntable = linkedHauntable;
        followObject.objectToFollow = linkedHauntable.gameObject;
        Recalculate();
    }

    // Update is called once per frame
    void Update()
    {
        Recalculate();
    }

    void Recalculate()
    {
        int displayedAmt = _hauntable.DisplayedHauntJuice;
        numberText.text = displayedAmt.ToString();
        
        // Turn the amount into a number between 0 and 1, so we can feed it to the progress bars
        _normalizedProgress = 1 - Mathf.Clamp01((float) displayedAmt / _hauntable.hauntCost);

        _barScale = Mathf.Lerp(_barScale, _normalizedProgress, Time.unscaledDeltaTime * progressLerpSpeed);
        progressBarMask.localScale = _progressBarMaskInitScale * _barScale;
    }

    public void ShowFull()
    {
        onShowFull.Invoke();
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
