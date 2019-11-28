using ShootyGhost;
using UnityEngine;
using UnityEngine.Events;

public class HauntGlob : MonoBehaviour
{
    public float value = .1f;
    [Tooltip("This acts as a scale multiplier. Total scale is a product of this and the value of the glob")]
    public float scale = 1;
    public UnityEvent onGrabbed;
    
    // Start is called before the first frame update
    void Start()
    {
        ScaleToValue();
    }

    public void SetValue(float newValue)
    {
        value = newValue;
        ScaleToValue();
    }

    void ScaleToValue()
    {
        transform.localScale = Vector3.one * scale * value;
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
