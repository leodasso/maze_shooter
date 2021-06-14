using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Arachnid {

	/// <summary>
	/// Base class for any scriptable objects that hold a value.
	/// For a more concrete example, think of a scriptable object that sits in asset folder 
	/// and holds the value for HP (value:5) which is then referenced by components.
	/// </summary>
	/// <typeparam name="T">Type of the value this holds</typeparam>
	public abstract class ValueAsset<T> : ScriptableObject
	{
		[ToggleLeft]
        public bool readOnly;

		[SerializeField, ShowInInspector, OnValueChanged("RaiseEvents")]
		protected T myValue;

		[AssetsOnly]
        public List<GameEvent> onValueChange;

        [MultiLineProperty(5)]
        public string comments;

		public T Value
		{
			get { return myValue; }
			set {
				// If the value is changing, raise the onValueChange events
				if (ValueHasChanged(value))
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

		void RaiseEvents() {
			if (!Application.isPlaying) return;
			foreach (var e in onValueChange) e.Raise();
		}

		protected abstract bool ValueHasChanged(T newValue);
	}

}
