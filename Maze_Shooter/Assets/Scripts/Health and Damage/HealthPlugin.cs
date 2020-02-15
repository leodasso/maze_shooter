using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Health))]
public class HealthPlugin : MonoBehaviour
{
    [ReadOnly]
    public Health health;
    bool _delegatesAdded;
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        FindHealthComponent();
        AddDelegates();
    }
    
    protected void FindHealthComponent()
    {
        health = GetComponent<Health>();
        if (!health)
            Debug.LogWarning("Health Event component " + name +
                             " requires a Health component on the same object in order to function.");
    }

    protected void AddDelegates()
    {
        if (_delegatesAdded) return;
        if (!health) FindHealthComponent();
        health.onDamaged += Damaged;
        health.onHealed += Healed;
        _delegatesAdded = true;
    }

    protected virtual void Damaged(int newHp)
    {
        if (!health) FindHealthComponent();
    }

    protected virtual void Healed(int newHp)
    {
        if (!health) FindHealthComponent();
    }
}