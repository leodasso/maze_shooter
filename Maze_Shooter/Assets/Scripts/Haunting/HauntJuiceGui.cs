using ShootyGhost;
using UnityEngine;
using TMPro;

public class HauntJuiceGui : MonoBehaviour
{
    public TextMeshPro numberText;
    public FollowObject followObject;
    public Animator animator;
    public string showTrigger;
    public string hideTrigger;

    Haunter _haunter;
    bool _showing;

    void Start()
    {
        Show();
    }

    void Update()
    {
        if (!_haunter) return;
        numberText.text = _haunter.hauntJuice.ToString();
    }

    public void Show()
    {
        // This logic is to prevent tons of trigger calls being sent to the animator,
        // which seems to confuse it.
        if (_showing) return;
        _showing = true;
        animator.SetTrigger(showTrigger);
    }

    public void Hide()
    {
        if (!_showing) return;
        _showing = false;
        animator.SetTrigger(hideTrigger);
    }

    public void Init(Haunter newHaunter)
    {
        _haunter = newHaunter;
        followObject.objectToFollow = newHaunter.gameObject;
    }
}