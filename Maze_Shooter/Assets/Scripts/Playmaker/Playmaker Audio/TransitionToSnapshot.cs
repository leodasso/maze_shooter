using UnityEngine;
using UnityEngine.Audio;
using Arachnid;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Audio)]
	[Tooltip("Transitions to an audio mixer snapshot.")]
	public class TransitionToSnapshot : FsmStateAction
	{
		[RequiredField]
        public AudioMixerSnapshot snapshot;
		public FloatReference transitionTime;

		public override void Reset()
		{
			snapshot = null;
		}

		public override void OnEnter()
		{
			if (snapshot != null)
			{
				snapshot.TransitionTo(transitionTime.Value);
			}
			
			Finish();
		}
	}
}