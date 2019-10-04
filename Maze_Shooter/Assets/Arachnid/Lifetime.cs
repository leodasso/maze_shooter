using System.Collections;
using System.Collections.Generic;
using Arachnid;
using Sirenix.OdinInspector;
using UnityEngine;

[TypeInfoBox("Destroys this gameobject after the lifetime")]
public class Lifetime : MonoBehaviour
{
    [Tooltip("This object's lifetime (in seconds)")]
    public FloatReference lifetime;

    [Tooltip("Do an oldschool arcade flash when about to disappear")]
    public bool flashOut = false;

    [ShowIf("flashOut")]
    public Flasher flasher;

    [ShowIf("flashOut"), Tooltip("How long will it flash for before disappearing?")]
    public FloatReference flashOutDuration;
    
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(KillMe), lifetime.Value);

        if (flashOut)
        {
            // get the time to wait before beginning to flash
            float beginFlashTime = Mathf.Min(lifetime.Value, flashOutDuration.Value);
            Invoke(nameof(BeginFlashing), lifetime.Value - beginFlashTime);
        }
    }

    void KillMe()
    {
        Destroy(gameObject);
    }

    void BeginFlashing()
    {
        flasher?.SetFlashing(true);
    }
}
