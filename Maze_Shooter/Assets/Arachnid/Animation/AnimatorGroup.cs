using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AnimatorGroup : MonoBehaviour
{
    [ToggleLeft]
    public bool triggerOnStart;

    [MinValue(0), Tooltip("Time between setting each trigger in the group.")]
    public float interval = 0;
    
    [ShowIf("triggerOnStart")]
    public string startTrigger;
    
    public List<Animator> animators = new List<Animator>();

    void Start()
    {
        if (triggerOnStart) SetTrigger(startTrigger);
    }

    [Button]
    void GetAnimatorsFromChildren()
    {
        animators.Clear();
        animators.AddRange(GetComponentsInChildren<Animator>());
    }

    [Button]
    public void SetTrigger(string trigger)
    {
        StartCoroutine(IntervalSetTrigger(trigger));
    }

    IEnumerator IntervalSetTrigger(string trigger)
    {
        foreach (Animator anim in animators)
        {
            anim.SetTrigger(trigger);
            yield return new WaitForSeconds(interval);
        }
    }
}
