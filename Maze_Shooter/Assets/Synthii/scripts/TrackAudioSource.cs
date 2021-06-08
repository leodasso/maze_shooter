using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
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

		// what track volumes get set to
		TrackVolumes trackVolumes;

		// raw output - thing that does the lerping
		TrackVolumes trackVolumesOutput;

		[Tooltip("The music zone that I'm playing")]
		public MusicZone musicZone;

		Track track;

		List<AudioSource> sources = new List<AudioSource>();

		float _mainVolume = 1;

		public Track MyTrack => musicZone != null ? musicZone.musicTrack : track;

		float fadeInTime;
		float trackVolumesLerpSpeed => 1 / fadeInTime;


		void Update() 
		{
			if (!MyTrack) return;
			if (audioState != AudioState.Playing) return;

			if (sources[0].time >= MyTrack.EndLoopTime) {
				audioState = AudioState.FadeToLoop;
				Loop();
			}

			trackVolumesOutput = TrackVolumes.Lerp(trackVolumesOutput, trackVolumes, Time.deltaTime * trackVolumesLerpSpeed);
			UpdateAudioSourcesVolume();
		}


		public void LerpMainVolume(float newVolume, float duration) 
		{
			StopAllCoroutines();
			StartCoroutine(LerpMainVolumeRoutine(newVolume, duration, null));
		}

		public void LerpMainVolume(float newVolume, float duration, System.Action onLerpComplete) 
		{
			StopAllCoroutines();
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
		/// Change the music zone I'm playing for.
		/// This ONLY supports swapping between zones that share the same track! 
		/// </summary>
		public void ChangeZones(MusicZone newMusicZone, float newFadeTime = 1)
		{
			if (newMusicZone == musicZone) return;

			string oldName = musicZone ? musicZone.name : "null";
			Debug.Log("Track " + MyTrack.name + " is changing zones from " + oldName + " to " + newMusicZone.name);
			if (newMusicZone.musicTrack != MyTrack) {
				Debug.LogError(name + " trying to change music zones, but the current and new zones don't " + 
				"have the same tracks.", gameObject);
				return;
			}

			musicZone = newMusicZone;
			trackVolumes = newMusicZone.GenerateVolumes();
			fadeInTime = newFadeTime;
		}

		public void BuildAudioSources(MusicZone newMusicZone, AudioMixerGroup mixerGroup) 
		{
			musicZone = newMusicZone;
			track = musicZone.musicTrack;
			trackVolumes = trackVolumesOutput = newMusicZone.GenerateVolumes();

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
				newAudioSource.outputAudioMixerGroup = mixerGroup;

				sources.Add(newAudioSource);
			}

			Play(musicZone.fadeInTime);
		}


		void Loop() 
		{
			// Tell music player to duplicate this object
			GetComponentInParent<MusicPlayer>().LoopTrack(musicZone);

			// fade this object out and destroy
			float fadeTime = Mathf.Clamp(5, 0, MyTrack.croppedEndTime);
			LerpMainVolume(0, fadeTime, DestroyMe);
		}

		void DestroyMe() {Destroy(gameObject);}


		void UpdateAudioSourcesVolume() 
		{
			for (int i = 0; i < sources.Count; i++)
				sources[i].volume = mainVolume * trackVolumesOutput.volumes[i];
		}


		[ButtonGroup]
		public void Play(float fadeDuration = 1) 
		{
			if (!Application.isPlaying || !musicZone) return;
			PlaySources();
			LerpMainVolume(1, fadeDuration);
		}

		[ButtonGroup]
		public void Stop(float fadeDuration = 1) 
		{
			if (!Application.isPlaying || !musicZone) return;
			Debug.Log(name + " is stopping.");
			LerpMainVolume(0, fadeDuration, StopSources);
		}

		[ButtonGroup]
		public void Pause(float fadeDuration = 1) 
		{
			if (!Application.isPlaying || !musicZone) return;
			Debug.Log(name + " is pausing.");
			LerpMainVolume(0, fadeDuration, PauseSources);
		}


		// The below functions just apply their named playback function to 
		// all the audio sources for this track.
		void PlaySources() 
		{
			// allow for play to be called multiple times without interruption
			if (audioState == AudioState.Playing) return;
			audioState = AudioState.Playing;

			Debug.Log(name + " playing all audio sources.");

			foreach (var source in sources)
				source.Play();
		}

		void PauseSources() 
		{
			audioState = AudioState.Paused;
			Debug.Log(name + " pausing all audio sources.");

			foreach (var source in sources)
				source.Pause();
		}	

		void StopSources() 
		{
			audioState = AudioState.Stopped;
			Debug.Log(name + " stopping all audio sources.");

			foreach (var source in sources)
				source.Stop();
		}

	}
}