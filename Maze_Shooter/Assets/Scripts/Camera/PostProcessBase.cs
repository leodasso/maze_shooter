using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using Sirenix.OdinInspector;
   
public class PostProcessBase : MonoBehaviour
{
	PostProcessVolume _volume;
	Volume _urpVolume;

	protected float weight {
		get {
			return weightInternal;  
		}
		set {
			weightInternal = value;
			if (_volume)
				_volume.weight = value;

			if (_urpVolume)
				_urpVolume.weight = value;
		}
	}

	float weightInternal;

	void Awake()
    {
        _volume = GetComponent<PostProcessVolume>();
		_urpVolume = GetComponent<Volume>();
    }
}