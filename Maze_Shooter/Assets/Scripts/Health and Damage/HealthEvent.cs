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
public class HealthEvent : HealthPlugin
{
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

    protected override void Start()
    {
        base.Start();        
        UpdateHpOfEvent();
    }

    protected override void Damaged(int newHp)
    {
        base.Damaged(newHp);
        CheckEvent(newHp);
    }

    /// <summary>
    /// If mode is set to percentage, get's the HP value that lines up to that percentage
    /// </summary>
    void UpdateHpOfEvent()
    {
        if (modeIsHitPoints) return;
        if (!health) FindHealthComponent();
        if (!health) return;

        hitPointsOfEvent = Mathf.RoundToInt(health.maxHearts.Value * percentageOfEvent);
    }

    void ClampHitPoints()
    {
        if (!health) FindHealthComponent();
        if (!health) return;
        hitPointsOfEvent = Mathf.Clamp(hitPointsOfEvent, 0, health.maxHearts.Value);
    }

    void CheckEvent(int newHitPoints)
    {
        if (_invoked) return;
        if (newHitPoints > hitPointsOfEvent) return;
        _invoked = true;
        healthEvent.Invoke();
    }
}
