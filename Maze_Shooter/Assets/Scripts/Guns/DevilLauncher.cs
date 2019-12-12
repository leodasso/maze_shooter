using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using Sirenix.OdinInspector;

public class DevilLauncher : MonoBehaviour, IControllable
{
    [ReadOnly]
    public GameObject devilToLaunch;
    public GameObject launchPosition;
    public FloatReference launchSpeed;
    public GunBrain gunBrain;
    public Train devilTrain;
    [Tooltip("How far does the player's joystick need to lean before we prep a red devil for launch? (between 0 and 1)")]
    public FloatReference inputThreshhold;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyLeftStickInput(Vector2 input){}

    public void ApplyRightStickInput(Vector2 input)
    {
        // check if firing threshhold, place devil on launch position
        if (input.magnitude >= inputThreshhold.Value && !devilToLaunch)
            PrepDevilForLaunch();
        
        else if (input.magnitude < inputThreshhold.Value && devilToLaunch)
            ReturnPreppedDevil();
    }

    void PrepDevilForLaunch()
    {
        devilToLaunch = devilTrain.TakeFrontElement();
        if (!devilToLaunch) return;
        devilToLaunch.transform.parent = launchPosition.transform;
        devilToLaunch.transform.localPosition = Vector3.zero;
    }

    void ReturnPreppedDevil()
    {
        if (!devilToLaunch) return;
        devilToLaunch.transform.parent = null;
        devilTrain.PlaceInFront(devilToLaunch);
        devilToLaunch = null;
    }

    public void DoActionAlpha()
    {
        LaunchDevil();
    }

    void LaunchDevil()
    {
        if (!devilToLaunch)
            PrepDevilForLaunch();
        
        if (!devilToLaunch)
            return;

        Vector3 launchVector = gunBrain.GetAimVector();
        devilToLaunch.transform.parent = null;

        Devil devil = devilToLaunch.GetComponent<Devil>();
        devil.Launch(launchVector * launchSpeed.Value);

        devilToLaunch = null;
    }

    public string Name()
    {
        return "devil launcher: " + name;
    }
}
