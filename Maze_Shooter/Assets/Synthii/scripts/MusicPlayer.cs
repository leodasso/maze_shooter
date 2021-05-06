using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Sirenix.OdinInspector;

namespace Synthii
{
	public class MusicPlayer : MonoBehaviour
	{
		[SerializeField]
		TrackAudioSource currentTrackSource;

		[SerializeField]
		AudioMixerGroup mixerGroup;

		Dictionary<Track, TrackAudioSource> trackSources = new Dictionary<Track, TrackAudioSource>();
		

		public void Play(MusicZone zone) 
		{
			if (currentTrackSource) {
				// Ignore request if the zone is already playing
				if (currentTrackSource.musicZone == zone) 
						return;

				float fadeOutTime = currentTrackSource.musicZone ? currentTrackSource.musicZone.fadeOutTime : 1;

				// Pause the currently playing audio source
				if (zone.musicTrack != currentTrackSource.musicZone.musicTrack) 
					currentTrackSource.Pause(fadeOutTime);
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



		public static void PlayTrack(Track track) 
		{

		}

		void Awake() 
		{
			Object.DontDestroyOnLoad(gameObject);
		}

		// Update is called once per frame
		void Update()
		{
			
		}
	}
}