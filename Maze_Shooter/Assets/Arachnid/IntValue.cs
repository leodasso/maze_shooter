using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Arachnid
{

	[CreateAssetMenu(menuName ="Arachnid/Int Value")]
	public class IntValue : ScriptableObject
	{
		[ToggleLeft]
		public bool readOnly;
		[SerializeField, ShowInInspector, OnValueChanged("RaiseEvents")]
		int myValue;
        
		public int Value
		{
			get { return myValue; }
			set {
				// If the value is changing, raise the onValueChange events
				if (value != myValue)
				{
					if (readOnly)
					{
						Debug.LogWarning(name + " value can't be set because it's readonly.", this);
						return;
					}
					myValue = value;
					RaiseEvents();
				}
			}
		}

		[AssetsOnly]
		public List<GameEvent> onValueChange;
		[MultiLineProperty(5)]
		public string comments;

		/// <summary>
		/// Increases the value by the given amount
		/// </summary>
		public void IterateValue (int amount)
		{
			Value += amount;
		}

		/// <summary>
		/// Resets the value back to zero.
		/// </summary>
		public void ResetValue()
		{
			Value = 0;
		}

		void RaiseEvents() {
			if (!Application.isPlaying) return;
			foreach (var e in onValueChange) e.Raise();
		}
	}
}