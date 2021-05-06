using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Synthii
{
	public class MusicPlayer : MonoBehaviour
	{
		[SerializeField]
		Track currentTrack;

		[SerializeField]
		TrackAudioSource currentTrackSource;

		[SerializeField, ReadOnly]
		Dictionary<Track, TrackAudioSource> trackSources = new Dictionary<Track, TrackAudioSource>();
		

		public void Play(MusicZone zone) 
		{
			// Ignore request if the zone is already playing
			if (currentTrackSource && currentTrackSource.musicZone == zone) 
					return;

			TrackAudioSource newTrackAudioSource = null;

			// If an audio source stack has already been built for the 
			// music track of this zone, just change zones on it.
			if (trackSources.TryGetValue(zone.musicTrack, out newTrackAudioSource)) {
				newTrackAudioSource.ChangeZones(zone);
				newTrackAudioSource.Play(1);
				return;
			}

			// if we don't have an audio source stack for this track yet, 
			// build one and set it as the new current track
			newTrackAudioSource = BuildNewTrackAudioSource(zone);
			newTrackAudioSource.BuildAudioSources(zone);
			currentTrackSource = newTrackAudioSource;
		}

		TrackAudioSource BuildNewTrackAudioSource(MusicZone newZone) 
		{
			GameObject newGO = new GameObject();
			newGO.name = "track source: " + newZone.musicTrack.name;
			newGO.transform.parent = transform;
			newGO.transform.localPosition = Vector3.zero;
			TrackAudioSource newTrack = newGO.AddComponent<TrackAudioSource>();
			trackSources.Add(newZone.musicTrack, newTrack);
			return newTrack;
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