using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

// TODO
// lerp devils to head
// if fired is pressed before the lerp is done, they should teleport to fire position

public class DevilLauncher : MonoBehaviour, IControllable
{
    public enum LaunchState
    {
        Empty,
        Transporting,
        Ready
    }
    [ReadOnly, Tooltip("The devil currently being prepped to launch.")]
    public GameObject devilToLaunch;
    public GameObject launchPosition;
    public GunBrain gunBrain;
    
    [Tooltip("The train is like a conga line of devils that follow behind you. It acts as the ammo clip for this launcher.")]
    public Train devilTrain;
    
    [Space, Tooltip("How far does the player's joystick need to lean before we prep a red devil for launch? (between 0 and 1)")]
    public FloatReference inputThreshhold;
    
    [Tooltip("Used to calculate the velocity of the launched devil (aim direction * launch speed = velocity)")]
    public FloatReference launchSpeed;
    
    [Tooltip("How long it takes to transport a devil from the train to on top of head (launch position)")]
    public FloatReference transportTime;
    
    public LaunchState launchState = LaunchState.Empty;

    public UnityEvent onLaunch;
    
    

    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public void ApplyRightStickInput(Vector2 input)
    {
        // check if firing threshhold, place devil on launch position
        if (input.magnitude >= inputThreshhold.Value && launchState == LaunchState.Empty)
            BeginTransport();
        
        else if (input.magnitude < inputThreshhold.Value && launchState == LaunchState.Ready)
            ReturnPreppedDevil();
    }

    /// <summary>
    /// Begins transporting a devil from the train to the top of head to prepare for launch.
    /// Handles empty train situation as well.
    /// </summary>
    void BeginTransport()
    {
        if (devilTrain.EmptyTrain)
            return;

        if (launchState == LaunchState.Empty)
        {
            StartCoroutine(MoveDevilToHead());
            launchState = LaunchState.Transporting;
        }
    }

    IEnumerator MoveDevilToHead()
    {
        float progress = 0;
        devilToLaunch = devilTrain.TakeFrontElement();
        Vector3 initPos = devilToLaunch.transform.position;
        while (progress < 1)
        {
            devilToLaunch.transform.position = Vector3.Lerp(initPos, launchPosition.transform.position, progress);
            progress += Time.deltaTime / transportTime.Value;
            yield return new WaitForEndOfFrame();
        }
        
        // Finalize devil position
        devilToLaunch.transform.parent = launchPosition.transform;
        devilToLaunch.transform.localPosition = Vector3.zero;
        launchState = LaunchState.Ready;
    }

    void ReturnPreppedDevil()
    {
        if (!devilToLaunch) return;
        devilToLaunch.transform.parent = null;
        devilTrain.PlaceInFront(devilToLaunch);
        devilToLaunch = null;
        launchState = LaunchState.Empty;
    }

    public void DoActionAlpha()
    {
        LaunchDevil();
    }

    
    void LaunchDevil()
    {
        if (!devilToLaunch || launchState != LaunchState.Ready)
            return;
        
        Vector3 launchVector = gunBrain.GetAimVector();
        devilToLaunch.transform.parent = null;

        Devil devil = devilToLaunch.GetComponent<Devil>();
        devil.Launch(launchVector * launchSpeed.Value);

        devilToLaunch = null;
        launchState = LaunchState.Empty;
        
        onLaunch.Invoke();
    }

    public void PickUpDevil(Devil devil)
    {
        devilTrain.PlaceInBack(devil.gameObject);
    }
    
    public void ApplyLeftStickInput(Vector2 input){}

    public string Name()
    {
        return "devil launcher: " + name;
    }
}
