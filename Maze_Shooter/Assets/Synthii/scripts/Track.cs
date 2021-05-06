using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Synthii
{
	[CreateAssetMenu(menuName = "Synthii/Track Stack")]
	public class Track : ScriptableObject
	{
		static float BeatsPerSecond(int bpm) => bpm / 60f;
		static float BeatDuration(int bpm) => 1 / BeatsPerSecond(bpm);


		[	OnValueChanged("UpdateTrackDetails"), 
			InlineEditor(inlineEditorMode: InlineEditorModes.SmallPreview), 
			AssetSelector(Paths = "Assets/Audio/Music")]
		public List<AudioClip> musicClips = new List<AudioClip>();


		[MinValue(1), OnValueChanged("UpdateBpm")]
		public int bpm = 80;

				[Tooltip("Number of beats to crop off the end of the track"), OnValueChanged("RecalculateCroppedEndTime")]
		public int croppedEndBeats = 2;

		[ReadOnly, Tooltip("Track length in seconds"), HorizontalGroup("details"), LabelWidth(80)]
		public float trackLength;

		[ReadOnly, Tooltip("Total number of beats in this track"), HorizontalGroup("details"), LabelWidth(80)]
		public float totalBeats;

		[ReadOnly]
		public float croppedEndTime;

		/// <summary>
		/// The timestamp in seconds that the song is marked to end and should loop back to beginning
		/// </summary>
		public float EndLoopTime => trackLength - croppedEndTime;

		float MaxBeats => Mathf.RoundToInt(totalBeats);

		bool MusicExists => musicClips != null && musicClips.Count > 0 &&  musicClips[0] != null;

		void UpdateTrackDetails() 
		{
			if (!MusicExists) return;
			trackLength = musicClips[0].length;

			UpdateBpm();
		}

		void UpdateBpm() 
		{
			if (!MusicExists) return;
			totalBeats = trackLength / BeatDuration(bpm);

			RecalculateCroppedEndTime();
		}

		void RecalculateCroppedEndTime() 
		{
			float remainder = totalBeats % 1;
			float total = remainder + croppedEndBeats;
			croppedEndTime = total * BeatDuration(bpm);
		}

		public void RecalculateAll() 
		{
			UpdateTrackDetails();
		}
	}
}