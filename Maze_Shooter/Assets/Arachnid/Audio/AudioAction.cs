using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AudioAction : MonoBehaviour
{
	[AssetsOnly]
	public AudioCollection audioCollection;

	[Range(0, 1), Tooltip("Multiplies by the volume of the audio collection.")]
	public float volume = 1;

	public float playDelay = 0;

	[ToggleLeft]
	public bool playOnAwake = false;
	[ToggleLeft]
	public bool playOnEnable = false;

	static GameObject _audioParent;

	static GameObject AudioParent()
	{
		if (_audioParent) return _audioParent;
		_audioParent = new GameObject("Audio");
		return _audioParent;
	}	

	// Use this for initialization
	void Start () 
	{
		if (playOnAwake) Play();
	}

	void OnEnable()
	{
		if (playOnEnable) Play();
	}

	[Button]
	public void Play()
	{
		if (playDelay < Mathf.Epsilon) ActualPlay();
		else StartCoroutine(DelayAndPlay());
	}

	IEnumerator DelayAndPlay() 
	{
		yield return new WaitForSeconds(playDelay);
		ActualPlay();
	}

	void ActualPlay() 
	{
		if (!GhostTools.SafeToInstantiate(gameObject))
		{
			Debug.LogWarning("Audio playing is only supporting while game is running.");
			return;
		}

		if (audioCollection == null)
		{
			Debug.LogWarning(name + " has no audio connection referenced!");
			return;
		}
		
		GameObject audioGO = new GameObject(audioCollection.name);
		audioGO.transform.parent = AudioParent().transform;
		audioGO.transform.position = transform.position;
		AudioSource newSource = audioGO.AddComponent<AudioSource>();
		newSource.spread = 180;
		newSource.dopplerLevel = 0;
		newSource.rolloffMode = AudioRolloffMode.Linear;
		newSource.clip = audioCollection.GetRandomClip();
		newSource.playOnAwake = false;
		newSource.outputAudioMixerGroup = audioCollection.mixerGroup;
		newSource.volume = audioCollection.volume * volume;
		newSource.pitch = audioCollection.Pitch();
		newSource.spatialBlend = 1;
		newSource.maxDistance = audioCollection.maxDistance;
		
		newSource.Play();
		
		Destroy(audioGO, audioCollection.audioLifetime);
	}
}
