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
	#if UNITY_EDITOR
	[UnityEditor.InitializeOnLoad]
	#endif
	public abstract class ValueAsset<T> : ScriptableObject, ISaveable
	{
		[ToggleLeft, SerializeField, PropertyOrder(-100)] 
    	protected bool debug;

		[TableColumnWidth(100, false)]
		[ToggleLeft, PropertyOrder(-50), SerializeField]
        bool readOnly;

		[TableColumnWidth(100, false)]
		[ PropertyOrder(-45), Space, ShowIf("CanSave"), ToggleLeft]
		[Tooltip("This value will be saved / loaded from save file on the gameMaster save() and load()")]
		public bool useSaveFile;

		[SerializeField]
		protected T myValue;

		[ SerializeField, Indent, ShowIf("useSaveFile")]
		[Tooltip("If the value is requested when it hasn't yet been added to the save file, this is what will be returned.")]
		protected T defaultValue;

		protected abstract string Prefix();

		protected abstract bool CanSave();

		int enableCount;

		bool DoesSave => CanSave() && useSaveFile;

		void OnEnable()
		{
			if (DoesSave) {
				PrepForSaveLoad();

				#if UNITY_EDITOR
				return;
				#endif

				// Scriptable objects are only loaded when the scene they are referenced in is loaded.
				// That means they need to load their value right when onEnable is called (unless it's running in Unity Editor)
				Load();
			}

			enableCount++;
		}

		void PrepForSaveLoad()
		{
			Debug.Log("Prepping for save/load " + name);
			GameMaster.allSaveables.Add(this);
		}

		[TableColumnWidth(150)]
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

		[ButtonGroup(), PropertyOrder(200), ShowIf("useSaveFile")]
		protected void LogSaveStatus()
		{
			Debug.Log(name + " value in save file: " + GameMaster.LoadFromCurrentFile(Prefix() + name, defaultValue, this));
		}

		[ButtonGroup(), PropertyOrder(200), ShowIf("useSaveFile")]
		protected virtual void ResetToDefault()
		{
			myValue = defaultValue;
			Save();
			LogSaveStatus();
		}


		public void Save()   
		{
			if (!CanSave() || !useSaveFile)
				return;

			if (debug)
				Debug.Log(name + " is being saved as value: " + myValue, this);

				SaveInternal();

			if (debug) 
				LogSaveStatus();
		}
		
		public void Load()
		{
			if (!CanSave() || !useSaveFile)
				return;
				
			LoadInternal();
			if (debug)
				Debug.Log(name + "'s value was loaded as " + myValue, this );
		}

		/// <summary>
		/// Processes myValue to save to file
		/// </summary>
		protected virtual void SaveInternal()
		{
			GameMaster.SaveToCurrentFile(Prefix() + name, myValue, this);
		}

		/// <summary>
		/// Process the raw save data into myValue
		/// </summary>
		protected virtual void LoadInternal()
		{
			myValue = GameMaster.LoadFromCurrentFile(Prefix() + name, defaultValue, this);
		}

		/// <summary>
		/// Checks if there's any data in the current save file for this value.
		/// </summary>
		public bool HasSavedValue()
		{
			return GameMaster.DoesKeyExist(Prefix() + name);
		}

		public string Info()
		{
			return name + ": " + myValue.ToString() + "    enabled " + enableCount + " times.";
		}
	}
}
