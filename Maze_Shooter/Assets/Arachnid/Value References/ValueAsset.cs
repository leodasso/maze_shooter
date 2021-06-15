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
		[TableColumnWidth(100, false)]
		[ToggleLeft, PropertyOrder(-50)]
        public bool readOnly;

		[SerializeField, HideInInspector]
		protected T myValue;

		[TableColumnWidth(150)]
		[ShowInInspector, PropertyOrder(-40)]
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

					ProcessValueChange(value);

					myValue = value;
					RaiseEvents();
				}
			}
		}

		[AssetsOnly, PropertyOrder(200)]
        public List<GameEvent> onValueChange;

        [MultiLineProperty(4), HideLabel, Title("Comments", bold: false)]
        public string comments;

		protected abstract void ProcessValueChange(T newValue);

		protected void RaiseEvents() 
		{
			RaiseEvents(onValueChange);
		}

		protected void RaiseEvents(List<GameEvent> eventList) 
		{
			if (!Application.isPlaying) return;
			foreach (var e in eventList) e.Raise();
		}

		protected abstract bool ValueHasChanged(T newValue);
	}

}
