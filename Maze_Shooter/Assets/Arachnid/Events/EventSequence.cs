using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace Arachnid
{

	[CreateAssetMenu(menuName = "Arachnid/Event Sequence"),
	TypeInfoBox("Runs through a sequence of events, from the top of list to the bottom.")]
	public class EventSequence : ScriptableObject
	{
		[ToggleLeft]
		public bool debug;
		public List<SequenceStep> sequence = new List<SequenceStep>();
		public static EventSequence currentSequence;
		int _index = 0;

		
		
		[Button]
		public void BeginSequence()
		{
			if (sequence.Count < 1) return;
			if (currentSequence)
			{
				Debug.LogWarning(currentSequence.name + " is already running. Only one sequence can be running at a time.", this);
				return;
			}
			currentSequence = this;
			_index = 0;
			ExecuteStep(0);
		}
		

		void ExecuteStep(int unitIndex)
		{
			if (unitIndex < 0 || unitIndex >= sequence.Count)
			{
				Debug.LogError("Tried to execute a sequence step outside the range of the steps list.");
				return;
			}
			
			if (debug) Debug.Log(name + " is executing step at index " + unitIndex, this);
			
			sequence[unitIndex].Execute();
			
			// If this step is timed, then start the next step after a given amount of time
			if (sequence[unitIndex].duration <= 0) return;
			CoroutineHelper.NewCoroutine(DelayedAdvanceSequence(sequence[unitIndex].duration));
		}

		
		IEnumerator DelayedAdvanceSequence(float delayTime)
		{
			yield return new WaitForSecondsRealtime(delayTime);
			Instance_AdvanceSequence();
		}

		public static void AdvanceSequence()
		{
			if (!currentSequence) return;
			currentSequence.Instance_AdvanceSequence();
		}
		
		/// <summary>
		/// Moves the sequence to the next step. If it's at the last step already, ends the sequence.
		/// </summary>
		void Instance_AdvanceSequence()
		{
			if (debug) Debug.Log(name + " is advancing from step index " + _index + " to step index " + (_index + 1), this);
			_index++;
			if (_index >= sequence.Count)
			{
				EscapeSequence();
				return;
			}
			
			ExecuteStep(_index);
		}

		
		public void EscapeSequence()
		{
			if (currentSequence != this)
			{
				Debug.LogWarning("Sequence " + name + " has been told to escape, however it's not the current sequence.");
				return;
			}
			
			if (debug) Debug.Log("Sequence " + name + " is ending.", this);

			currentSequence = null;
		}


		[System.Serializable]
		public class SequenceStep
		{
			[BoxGroup("s", centerLabel:true, GroupName = "Sequence Step"), 
			 Tooltip("Duration of this step. If left at 0, sequence will remain at this step until it's told to move on."), MinValue(0)]
			public float duration = 2;
			
			[TabGroup("s/e", "game events")]
			public List<GameEvent> events = new List<GameEvent>();

			[DrawWithUnity, TabGroup("s/e", "unity event")]
			public UnityEvent uEvent;

			public void Execute()
			{
				foreach (var e in events) e.Raise();
				uEvent.Invoke();
			}
		}
	}
}