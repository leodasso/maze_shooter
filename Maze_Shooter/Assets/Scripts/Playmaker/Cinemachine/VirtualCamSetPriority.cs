using HutongGames.PlayMaker;
using UnityEngine;

[ActionCategory(ActionCategory.Camera)]
public class VirtualCamSetPriority : FsmStateAction
{
    [HutongGames.PlayMaker.Tooltip("The change in camera priority")]
    public int priorityDelta;

    public Cinemachine.CinemachineVirtualCamera virtualCamera;

    public override void OnEnter()
    {
        if (virtualCamera)
            virtualCamera.Priority += priorityDelta;

        Finish();
    }
}
