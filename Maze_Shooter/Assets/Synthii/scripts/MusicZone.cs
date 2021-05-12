using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Synthii {
	public class MusicZone : MonoBehaviour
	{
		[ToggleLeft, Tooltip("Will play as the default for this scene.")]
		public bool isGlobal;

		[Tooltip("Higher number takes priority over lower number")]
		public int priority = 0;

		[Space]
		public Track musicTrack;
		public List<TrackLayer> layers = new List<TrackLayer>();

		[MinValue(0.01)]
		public float fadeInTime = 3;
		[MinValue(0.01)]
		public float fadeOutTime = 6;

		// Start is called before the first frame update
		void Start()
		{
			if (isGlobal) 
				MusicPlayer.SetGlobal(this);
		}

		[ButtonGroup]
		public void Enter() 
		{
			MusicPlayer.Play(this);
		}

		[ButtonGroup]
		public void Exit()
		{
			MusicPlayer.Stop(this);
		}

		[Button]
		void GenerateTrackLayers() 
		{
			if (!musicTrack) return;
			layers.Clear();
			for (int i = 0; i < musicTrack.musicClips.Count; i++) {
				TrackLayer newLayer = new TrackLayer();
				newLayer.clip = musicTrack.musicClips[i];
				layers.Add(newLayer);
			}
		}

		void OnDisable() 
		{
			if (isGlobal) 
				MusicPlayer.RemoveGlobal(this);
			Exit();
		}

		
		/// <summary>
		/// Generates the volumes as a nice struct
		/// </summary>
		public TrackVolumes GenerateVolumes() 
		{
			return new TrackVolumes(layers);
		}
	}

	[System.Serializable]
	public class TrackLayer
	{
		[HideLabel, ReadOnly, InlineEditor(inlineEditorMode: InlineEditorModes.SmallPreview)]
		public AudioClip clip;
		[Range(0, 1), LabelWidth(80)]
		public float volume = 1;
	}

	/// <summary>
	/// This class helps keep track of the volumes of each layer.
	/// Also supports lerping of layers' volume  as long as there are the same count of layers.
	/// </summary>
	public struct TrackVolumes
	{
		[Range(0,1)]
		public List<float> volumes;

		public TrackVolumes(List<TrackLayer> trackLayers)
		{
			volumes = new List<float>();
			for (int i = 0; i < trackLayers.Count; i++)
				volumes.Add(trackLayers[i].volume);
		}

		public TrackVolumes(TrackVolumes original) 
		{
			volumes = new List<float>(original.volumes);
		}

		public static TrackVolumes Lerp(TrackVolumes A, TrackVolumes B, float progress) 
		{
			if (A.volumes.Count != B.volumes.Count) 
				throw new System.Exception("Track volumes must have the same number of volumes to lerp!");
			
			TrackVolumes lerpResult = new TrackVolumes(A);
			for (int i = 0; i < A.volumes.Count; i++) 
				lerpResult.volumes[i] = Mathf.Lerp(A.volumes[i], B.volumes[i], progress);

			return lerpResult;
		}

		public override string ToString()
		{
			string s = "";
			foreach(float f in volumes) 
				s += "layer " + volumes.IndexOf(f) + ": " + f;
			
			return s;
		}
	}
}