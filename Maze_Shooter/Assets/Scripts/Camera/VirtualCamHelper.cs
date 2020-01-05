using System.Collections;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

[TypeInfoBox("Provides a number of helper functions for interfacing with virtual cameras.")]
[RequireComponent(typeof(CinemachineVirtualCamera))]
public class VirtualCamHelper : MonoBehaviour
{
    [MinValue(0.1f), Tooltip("When the priority of the attached virtual camera is overridden, how long before it returns" +
                             "to its previous value.")]
    public float priorityOverrideDuration = 5;

    [Space]
    public bool copyFollowTarget;
    [ShowIf("copyFollowTarget")]
    public CinemachineVirtualCamera cameraToCopyTarget;
    
    CinemachineVirtualCamera _vCam;
    
    // Start is called before the first frame update
    void Start()
    {
        _vCam = GetComponent<CinemachineVirtualCamera>();
        if (!_vCam)
        {
            Debug.LogWarning("No virtual cam is attached to the virtual cam helper!", gameObject);
            enabled = false;
        }
    }

    void Update()
    {
        if (copyFollowTarget)
            CopyFollowTarget();
    }

    void CopyFollowTarget()
    {
        if (!cameraToCopyTarget) return;
        _vCam.Follow = cameraToCopyTarget.Follow;
    }

    public void OverridePriority(int newPriority)
    {
        int originalPriority = _vCam.Priority;
        _vCam.Priority = newPriority;
        StartCoroutine(SetPriority(originalPriority, priorityOverrideDuration));
    }

    IEnumerator SetPriority (int priority, float delay)
    {
        yield return new WaitForSeconds(delay);
        _vCam.Priority = priority;
    }
}