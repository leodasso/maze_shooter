using ShootyGhost;
using UnityEngine;
using UnityEngine.Events;

public class HauntPacket : MonoBehaviour
{
    public SmartMissile3D smartMissile;
    public float speed = 15;
    public new Rigidbody rigidbody;

    public UnityEvent onMergeWithTarget;
    public UnityEvent onLeaveTarget;

    GameObject _target;
    Haunter _haunter;
    Hauntable _targetHauntable;
    bool _exitedHaunter;

    void OnTriggerEnter(Collider other)
    {
        // Only consider trigger interactions with the target
        if (other.gameObject != _target) return;
        
        Hauntable otherHauntable = other.GetComponent<Hauntable>();
        if (otherHauntable)
            MergeWithTarget(otherHauntable);

        Haunter otherHaunter = other.GetComponent<Haunter>();
        if (otherHaunter && _exitedHaunter)
            MergeWithHaunter(otherHaunter);
    }

    void OnTriggerExit(Collider other)
    {
        Haunter otherHaunter = other.GetComponent<Haunter>();
        if (otherHaunter)
            _exitedHaunter = true;
    }

    void MergeWithHaunter(Haunter haunter)
    {
        haunter.TakeBackHauntPacket(this);
        Destroy(gameObject);
    }

    void MergeWithTarget(Hauntable target)
    {
        target.AddHauntPacket(this);
        _haunter.OnPacketSuccess(target);
        onMergeWithTarget.Invoke();
    }

    public void Init(Haunter haunter, Hauntable hauntable)
    {
        _haunter = haunter;
        _targetHauntable = hauntable;
        SetTarget(_targetHauntable.transform);
    }

    public void SetTarget(Transform newTarget)
    {
        _target = newTarget.gameObject;
        smartMissile.SetCustomTarget(newTarget);
        rigidbody.velocity = Random.onUnitSphere * speed;
    }

    public void ReturnToHaunter()
    {
        _targetHauntable.LoseHauntPacket(this);
        SetTarget(_haunter.transform);
        onLeaveTarget.Invoke();
    }
}
