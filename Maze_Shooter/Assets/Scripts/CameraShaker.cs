using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraShaker : MonoBehaviour
{
	CinemachineVirtualCamera _virtualCamera;
	CinemachineBasicMultiChannelPerlin _noise;
	
	public float frequencyDecay = 1;
	public float amplitudeDecay = 2;

	// Use this for initialization
	void Start ()
	{
		_virtualCamera = GetComponent<CinemachineVirtualCamera>();
		_noise = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		_noise.m_AmplitudeGain = Mathf.Lerp(_noise.m_AmplitudeGain, 0, Time.deltaTime * amplitudeDecay);
		_noise.m_FrequencyGain = Mathf.Lerp(_noise.m_FrequencyGain, 0, Time.deltaTime * frequencyDecay);
	}

	[Button]
	public void Shake(float intensity)
	{
		_noise.m_AmplitudeGain = Mathf.Max(_noise.m_AmplitudeGain, intensity);
		_noise.m_FrequencyGain = Mathf.Max(_noise.m_FrequencyGain, intensity);
	}
}
