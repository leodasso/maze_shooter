using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

[RequireComponent(typeof(Health))]
public class HealthEvent : MonoBehaviour
{
    [ReadOnly]
    public Health health;

    [OnValueChanged("ClampHitPoints")]
    public int hitPointsOfEvent;

    [DrawWithUnity]
    public UnityEvent healthEvent;

    bool _invoked;

    void Start()
    {
        FindHealthComponent();
        health.onDamaged += CheckEvent;
    }

    void FindHealthComponent()
    {
        health = GetComponent<Health>();
        if (!health) 
            Debug.LogWarning("Health Event component " + name + " requires a Health component on the same object in order to function.");
    }

    void ClampHitPoints()
    {
        if (!health) FindHealthComponent();
        if (!health) return;
        hitPointsOfEvent = Mathf.Clamp(hitPointsOfEvent, 0, health.hitPoints.Value);
    }

    void CheckEvent(int newHitPoints)
    {
        if (_invoked) return;
        if (newHitPoints > hitPointsOfEvent) return;
        _invoked = true;
        healthEvent.Invoke();
    }
}
