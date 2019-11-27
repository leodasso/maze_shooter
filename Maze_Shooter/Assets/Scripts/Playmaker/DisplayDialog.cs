using HutongGames.PlayMaker;
using UnityEngine;

public class DisplayDialog : FsmStateAction
{
    [RequiredField]
    public Dialog dialog;

    [HutongGames.PlayMaker.Tooltip("Use a specified dialog panel?")]
    public bool useSpecificPanel;

    public DialogPanel specifiedPanel;

    public override void Reset()
    {
        dialog = null;
    }

    public override void OnEnter()
    {
        if (dialog != null)
        {
            if (useSpecificPanel)
                specifiedPanel.ShowDialog(dialog);
            
            else
                dialog.Display();
        }
        
        Finish();
    }
}
