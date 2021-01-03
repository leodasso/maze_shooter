using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory(ActionCategory.Audio)]
    public class PlayAudioAction : FsmStateAction
    {
        [RequiredField]
        [CheckForComponent(typeof(AudioAction))]
        public FsmOwnerDefault gameObject;
        
        AudioAction _audioAction;
        
        
        public override void Reset()
        {
            base.Reset();
            gameObject = null;
            _audioAction = null;
        }

        public override void OnEnter()
        {
            // get the audio action component
            var go = Fsm.GetOwnerDefaultTarget(gameObject);
			
            if (go==null)
            {
                Finish();
                return;
            }
			
            _audioAction = go.GetComponent<AudioAction>();
			
            if (_audioAction==null)
            {
                Finish();
                return;
            }
            
            _audioAction.Play();
			Finish();
        }
    }
}