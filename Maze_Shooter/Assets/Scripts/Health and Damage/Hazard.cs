using Arachnid;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

[TypeInfoBox("Add this component to any object with a 2D collider, and it will deal damage to other iDestructible objects" +
             "that it comes into contact with.")]
public class Hazard : ContactBase
{
    [Tooltip("Layers that this hazard will do damage to")]
    public LayerMask layersToDamage;
	public HeartsRef damage;

	[SerializeField]
	UnityEvent onDoDamage;

    protected override void OnCollisionAction(Collision collision, Collider otherCol)
    {
        if (!enabled) return;
        if (!Math.LayerMaskContainsLayer(layersToDamage, otherCol.gameObject.layer)) return;
        
        IDestructible destructible = otherCol.GetComponent<IDestructible>();

		if (debug)
			Debug.Log(name + " dealing damage to " + otherCol.name);

		if (destructible != null) {
        	destructible.DoDamage(damage.Value, collision.GetContact(0).point, collision.GetContact(0).normal);
			onDoDamage.Invoke();
		}
    }

    protected override void OnTriggerAction(Collider other)
    {
        if (!enabled) return;
        if (!Math.LayerMaskContainsLayer(layersToDamage, other.gameObject.layer)) return;
        
        IDestructible destructible = other.GetComponent<IDestructible>();

		if (debug)
			Debug.Log(name + " dealing damage via OnTriggerAction to " + other.name);


		if (destructible != null) {
			destructible.DoDamage(damage.Value, (transform.position + other.transform.position)/2, 
				(transform.position - other.transform.position).normalized);
			onDoDamage.Invoke();
		}
    }

}
