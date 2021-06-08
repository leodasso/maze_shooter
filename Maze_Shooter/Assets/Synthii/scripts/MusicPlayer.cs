using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using Sirenix.OdinInspector;

namespace Synthii
{
	public class MusicPlayer : MonoBehaviour
	{
		[SerializeField, ToggleLeft]
		bool debug;

		[SerializeField, ReadOnly]
		TrackAudioSource currentTrackSource;

		[SerializeField]
		AudioMixerGroup mixerGroup;

		// Whenever active zones has a member added/removed, it re-creates ordered zones and orders it by priority
		HashSet<MusicZone> activeZones = new HashSet<MusicZone>();

		[ShowInInspector, ReadOnly, Tooltip("List of all active music zones in order of priority. The highest priority get index 0")]
		List<MusicZone> orderedZones = new List<MusicZone>();

		Dictionary<Track, TrackAudioSource> trackSources = new Dictionary<Track, TrackAudioSource>();

		static MusicPlayer instance;

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
		
		public static void EnterZone(MusicZone zone) 
		{
			GuaranteeInstance();
			instance.I_EnterZone(zone);
		}


		public static void ExitZone(MusicZone zone) 
		{
			GuaranteeInstance();
			instance.I_ExitZone(zone);
		}

		static void GuaranteeInstance() 
		{
			// Prevent objects from being spawned on un-play
			if (!GhostTools.SafeToInstantiate()) return;

			if (instance) return;
			instance = Instantiate(GameMaster.Get().musicPlayerPrefab).GetComponent<MusicPlayer>();
			DontDestroyOnLoad(instance.gameObject);
		}

		void I_EnterZone(MusicZone zone)
		{
			if (activeZones.Add(zone)) {
				if (debug)
					Debug.Log("Music zone " + zone + " was newly entered.");

				RecalculatePriority();
			}
		}

		void I_ExitZone(MusicZone zone)
		{
			if (activeZones.Remove(zone)) {
				if (debug)
					Debug.Log("Music zone " + zone + " was exited.");

				RecalculatePriority();
			}
		}

		void RecalculatePriority()
		{
			if (debug)
				Debug.Log("Recalculating music zone priorities...");

			orderedZones.Clear();
			orderedZones = activeZones.OrderByDescending(track => track.priority).ToList();

			// If there are no music zones left, just pause whatever is playing.
			if (orderedZones.Count < 1)
			{
				if (currentTrackSource)
					currentTrackSource.Pause();

				return;
			}

			// Play the highest priority music zone from the list (index 0)
			var topMusicZone  = orderedZones[0];
			Play(topMusicZone);
		}

		void Play(MusicZone zone) 
		{
			if (debug) Debug.Log("Attempting a play of " + zone.name);

			if (currentTrackSource) {
				// Pause the currently playing audio source if it's playing a different track than the requested zone
				if (zone.musicTrack != currentTrackSource.MyTrack) 
					currentTrackSource.Pause(CurrentTrackFadeOutTime());
			}

			TrackAudioSource newTrackAudioSource = null;

			// If an audio source stack has already been built for the 
			// music track of this zone, just change zones on it.
			if (trackSources.TryGetValue(zone.musicTrack, out newTrackAudioSource)) {
				newTrackAudioSource.ChangeZones(zone, zone.fadeInTime);
				newTrackAudioSource.Play(zone.fadeInTime);
				currentTrackSource = newTrackAudioSource;
				return;
			}

			// if we don't have an audio source stack for this track yet, 
			// build one and set it as the new current track
			newTrackAudioSource = BuildNewTrackAudioSource(zone);
			currentTrackSource = newTrackAudioSource;
		}



		void Stop(MusicZone zone) 
		{
			if (!currentTrackSource) return;
			if (currentTrackSource.musicZone != zone) return;

			currentTrackSource.Pause(CurrentTrackFadeOutTime());
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