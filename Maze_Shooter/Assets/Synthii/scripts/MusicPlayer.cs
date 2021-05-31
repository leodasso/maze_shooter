using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Sirenix.OdinInspector;

namespace Synthii
{
	public class MusicPlayer : MonoBehaviour
	{
		[SerializeField, ReadOnly]
		TrackAudioSource currentTrackSource;

		[SerializeField]
		AudioMixerGroup mixerGroup;

		Dictionary<Track, TrackAudioSource> trackSources = new Dictionary<Track, TrackAudioSource>();

		static MusicPlayer instance;

		MusicZone globalZone;

		void Awake() 
		{
			Object.DontDestroyOnLoad(gameObject);
			instance = this;
		}

		float CurrentTrackFadeOutTime ()
		{
			if (!currentTrackSource) return 1;
			return currentTrackSource.musicZone ? currentTrackSource.musicZone.fadeOutTime : 1;
		}
		
		public static void Play(MusicZone zone) 
		{
			GuaranteeInstance();
			instance.InstancePlay(zone);
		}


		public static void Stop(MusicZone zone) 
		{
			GuaranteeInstance();
			instance.InstanceStop(zone);
		}

		public static void SetGlobal(MusicZone zone) 
		{
			GuaranteeInstance();
			instance.InstanceSetGlobal(zone);
		}

		public static void RemoveGlobal(MusicZone zone)
		{
			GuaranteeInstance();
			instance.InstanceRemoveGlobal(zone);
		}

		static void GuaranteeInstance() 
		{
			if (instance) return;
			instance = Instantiate(GameMaster.Get().musicPlayerPrefab).GetComponent<MusicPlayer>();
			DontDestroyOnLoad(instance.gameObject);
		}

		void InstancePlay(MusicZone zone) 
		{
			Debug.Log("Attempting a play of " + zone.name);
			if (currentTrackSource) {
				// Ignore request if the zone is already playing
				if (currentTrackSource.musicZone == zone) {
					Debug.Log("   zone " + zone.name + " is already the current track! cancelling request.");
					return;
				}

				// Ignore lower quality requests
				if (currentTrackSource.musicZone && currentTrackSource.musicZone.priority > zone.priority) {
					Debug.Log("   Current playing zone " + currentTrackSource.musicZone.name + " has a higher priority than " + zone.name + ", cancelling.");
					return;
				}

				// Pause the currently playing audio source
				if (zone.musicTrack != currentTrackSource.MyTrack) 
					currentTrackSource.Pause(CurrentTrackFadeOutTime());
			}

			TrackAudioSource newTrackAudioSource = null;

			// If an audio source stack has already been built for the 
			// music track of this zone, just change zones on it.
			if (trackSources.TryGetValue(zone.musicTrack, out newTrackAudioSource)) {
				newTrackAudioSource.ChangeZones(zone);
				newTrackAudioSource.Play(zone.fadeInTime);
				currentTrackSource = newTrackAudioSource;
				return;
			}

			// if we don't have an audio source stack for this track yet, 
			// build one and set it as the new current track
			newTrackAudioSource = BuildNewTrackAudioSource(zone);
			currentTrackSource = newTrackAudioSource;
		}



		void InstanceStop(MusicZone zone) 
		{
			if (!currentTrackSource) return;
			if (currentTrackSource.musicZone != zone) return;

			currentTrackSource.Pause(CurrentTrackFadeOutTime());
			if (globalZone)
				InstancePlay(globalZone);
		}

		void InstanceSetGlobal(MusicZone zone)
		{
			Debug.Log("Attempting to set " + zone.name + " as the new global music...");
			if (globalZone) {
				if (globalZone.priority > zone.priority) {
					Debug.Log("   The current global zone " + globalZone.name + " has a higher priority than incoming zone " + zone.name + ", so cancelling.");
					return;
				}
			}

			globalZone = zone;
			
			InstancePlay(globalZone);
		}

		void InstanceRemoveGlobal(MusicZone zone) 
		{
			if (globalZone == zone) {
				Debug.Log("Removing track " + zone.name + " from global music.");
				globalZone = null;
				InstanceStop(zone);
			}
		}

		TrackAudioSource BuildNewTrackAudioSource(MusicZone newZone) 
		{
			GameObject newGO = new GameObject();
			newGO.name = "track source: " + newZone.musicTrack.name;
			newGO.transform.parent = transform;
			newGO.transform.localPosition = Vector3.zero;
			TrackAudioSource newTrack = newGO.AddComponent<TrackAudioSource>();
			newTrack.BuildAudioSources(newZone, mixerGroup);

			trackSources.Add(newZone.musicTrack, newTrack);
			return newTrack;
		}

		public void LoopTrack(MusicZone musicZone) 
		{
			// remove the old one from the dictionary
			var t = musicZone.musicTrack;
			trackSources.Remove(t);

			// re-create it
			var newTrackSource = BuildNewTrackAudioSource(musicZone);

			// if the looping track was the current playing, carryover that status
			if (currentTrackSource.musicZone == musicZone)
				currentTrackSource = newTrackSource;
		}
	}
}