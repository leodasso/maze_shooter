using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Synthii {

	public enum AudioState {
		Init,
		Playing,
		Stopped,
		Paused,
		FadeToLoop,		// Fading out for a loop of the song
	}

	[TypeInfoBox("This is an audio source for a music track. It is not meant to be manually added to anything - " +
	"it will be automatically created by Music Player component.")]
	public class TrackAudioSource : MonoBehaviour
	{

		public AudioState audioState = AudioState.Init;

		[PropertyRange(0, 1)]
		public float mainVolume {
			get {
				return _mainVolume;
			}
			set {
				_mainVolume = value;
				UpdateAudioSourcesVolume();
			}
		}

		TrackVolumes trackVolumes;

		[Tooltip("The music zone that I'm playing")]
		public MusicZone musicZone;

		List<AudioSource> sources = new List<AudioSource>();

		float _mainVolume = 1;

		float elapsedTime = 0;

		Track MyTrack => musicZone != null ? musicZone.musicTrack : null;

		void Update() 
		{
			elapsedTime = sources[0].time;
			if (!MyTrack) return;
			if (audioState != AudioState.Playing) return;

			if (elapsedTime >= MyTrack.EndLoopTime) {
				audioState = AudioState.FadeToLoop;
				Loop();
			}
		}


		public void LerpMainVolume(float newVolume, float duration) 
		{
			StartCoroutine(LerpMainVolumeRoutine(newVolume, duration, null));
		}

		public void LerpMainVolume(float newVolume, float duration, System.Action onLerpComplete) 
		{
			StartCoroutine(LerpMainVolumeRoutine(newVolume, duration, onLerpComplete));
		}

		IEnumerator LerpMainVolumeRoutine(float newVolume, float duration, System.Action onLerpComplete) 
		{
			float startVolume = mainVolume;
			float progress = 0;
			while (progress < 1) {
				progress += Time.unscaledDeltaTime / duration;
				mainVolume = Mathf.Lerp(startVolume, newVolume, progress);
				yield return null;
			}
			mainVolume = newVolume;

			if (onLerpComplete != null)
				onLerpComplete.Invoke();
		}

		/// <summary>
		/// Keeps the same music track, but fades the layer volumes
		/// from the settings in the previous zone to the new zone
		/// </summary>
		void LerpLayerVolumes(TrackVolumes newVolumes) 
		{
			StartCoroutine(LerpLayerVolumesRoutine(trackVolumes, newVolumes, 2));
		}

		IEnumerator LerpLayerVolumesRoutine(TrackVolumes start, TrackVolumes finish, float duration) 
		{
			float progress = 0;
			while (progress < 1) {
				progress += Time.unscaledDeltaTime / duration;
				trackVolumes = TrackVolumes.Lerp(start, finish, progress);
				UpdateAudioSourcesVolume();
				yield return null;
			}
			trackVolumes = finish;
			UpdateAudioSourcesVolume();
		}

		/// <summary>
		/// Change the music zone I'm playing for.
		/// This ONLY supports swapping between zones that share the same track! 
		/// </summary>
		public void ChangeZones(MusicZone newMusicZone, float fadeTime = 1)
		{
			if (newMusicZone == musicZone) return;
			if (newMusicZone.musicTrack != musicZone.musicTrack) {
				Debug.LogError(name + " trying to change music zones, but the current and new zones don't " + 
				"have the same tracks.", gameObject);
				return;
			}

			musicZone = newMusicZone;
			LerpLayerVolumes(newMusicZone.GenerateVolumes());
		}

		public void BuildAudioSources(MusicZone newMusicZone, float fadeInTime = 1) 
		{
			musicZone = newMusicZone;
			trackVolumes = newMusicZone.GenerateVolumes();

			// Create an audio source for each layer
			for (int i = 0; i < musicZone.layers.Count; i++) {

				var layer = musicZone.layers[i]; 

				// instantiate a source
				var newAudioSource = gameObject.AddComponent<AudioSource>();
				newAudioSource.spatialBlend = 0;
				newAudioSource.clip = layer.clip;
				newAudioSource.loop = false;
				newAudioSource.volume = 0;	
				newAudioSource.playOnAwake = false;	

				// TODO set the correct mixer group

				sources.Add(newAudioSource);
			}

			Play(fadeInTime);
		}


		void Loop() 
		{
			// Tell music player to duplicate this object
			GetComponentInParent<MusicPlayer>().LoopTrack(musicZone);

			// fade this object out and destroy
			LerpMainVolume(0, 1, DestroyMe);
		}

		void DestroyMe() {Destroy(gameObject);}


		void UpdateAudioSourcesVolume() 
		{
			for (int i = 0; i < sources.Count; i++)
				sources[i].volume = mainVolume * trackVolumes.volumes[i];
		}


		[ButtonGroup]
		public void Play(float fadeDuration = 1) 
		{
			if (!Application.isPlaying || !musicZone) return;
			LerpMainVolume(1, fadeDuration, PlaySources);
		}

		[ButtonGroup]
		public void Stop(float fadeDuration = 1) 
		{
			if (!Application.isPlaying || !musicZone) return;
			LerpMainVolume(0, fadeDuration, StopSources);
		}

		[ButtonGroup]
		public void Pause(float fadeDuration = 1) 
		{
			if (!Application.isPlaying || !musicZone) return;
			LerpMainVolume(0, fadeDuration, PauseSources);
		}


		// The below functions just apply their named playback function to 
		// all the audio sources for this track.
		void PlaySources() 
		{
			// allow for play to be called multiple times without interruption
			if (audioState == AudioState.Playing) return;
			audioState = AudioState.Playing;
			foreach (var source in sources)
				source.Play();
		}

		void PauseSources() 
		{
			audioState = AudioState.Paused;
			foreach (var source in sources)
				source.Pause();
		}	

		void StopSources() 
		{
			audioState = AudioState.Stopped;
			foreach (var source in sources)
				source.Stop();
		}

	}
}