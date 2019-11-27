using ShootyGhost;
using UnityEngine;
using UnityEngine.Events;

public class HauntGlob : MonoBehaviour
{
    public float value = .1f;
    public UnityEvent onGrabbed;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Haunter haunter = other.GetComponent<Haunter>();
        if (!haunter) return;
        
        onGrabbed.Invoke();
        haunter.AddJuice(value);
        Destroy(gameObject);
    }
}
