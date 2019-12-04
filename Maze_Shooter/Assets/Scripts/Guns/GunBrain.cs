using UnityEngine;
using Arachnid;
using Sirenix.OdinInspector;

/// <summary>
/// Controls rotation and firing of a gun based on a 2d directional input.
/// </summary>
public class GunBrain : MonoBehaviour, IControllable
{
    public GameObject gunRotator;
    public Gun gun;
    public FloatReference fireThreshhold;
    public Vector2 fireInput;

    [ReadOnly]
    public Quaternion aim;
    
    bool _firing;

    // Update is called once per frame
    void Update()
    {
        // check if the player is firing
        _firing = fireInput.magnitude >= fireThreshhold.Value;
        
        if (gun)
        {
            //gun.firing = _firing;
            //gun.fireRateIntensity = fireInput.magnitude;
        }
        
        if (!_firing) return;
		
        // Tell the gun where to fire
        float angle =  Math.AngleFromVector2(new Vector2(-fireInput.x, fireInput.y), -90);
        // declare aim below so other components can access it if they need to
        aim = Quaternion.Euler(0, angle, 0);
        gunRotator.transform.rotation = aim;
    }

    // This is required by the IControllable interface
    public void ApplyLeftStickInput(Vector2 input) {}

    public void ApplyRightStickInput(Vector2 input)
    {
        fireInput = input;
    }

    public string Name()
    {
        return "GunBrain: " + name;
    }
}