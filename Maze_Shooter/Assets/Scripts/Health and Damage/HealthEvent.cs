using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public enum HealthEventMode
{
    hitPoints = 0,
    percentage = 1,
}

[RequireComponent(typeof(Health))]
public class HealthEvent : MonoBehaviour
{
    [ReadOnly]
    public Health health;

    [Tooltip("Is this event based on remaining hit points or percentage of health?")]
    public HealthEventMode mode = HealthEventMode.hitPoints;

    [OnValueChanged("ClampHitPoints"), ShowIf("modeIsHitPoints")]
    public int hitPointsOfEvent;

    [Range(0, 1), HideIf("modeIsHitPoints"), OnValueChanged("UpdateHpOfEvent")]
    public float percentageOfEvent = .5f;

    [DrawWithUnity]
    public UnityEvent healthEvent;

    bool _invoked;

    public bool modeIsHitPoints => mode == HealthEventMode.hitPoints;

    void Start()
    {
        FindHealthComponent();
        health.onDamaged += CheckEvent;
        
        UpdateHpOfEvent();
    }

    void FindHealthComponent()
    {
        health = GetComponent<Health>();
        if (!health) 
            Debug.LogWarning("Health Event component " + name + " requires a Health component on the same object in order to function.");
    }

    void UpdateHpOfEvent()
    {
        if (modeIsHitPoints) return;
        if (!health) FindHealthComponent();
        if (!health) return;

        hitPointsOfEvent = Mathf.RoundToInt(health.hitPoints.Value * percentageOfEvent);
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
