using UnityEngine;
using Sirenix.OdinInspector;


namespace ShootyGhost {

	/// <summary>
	/// Volumes take a value (typically between 0 and 1) and convert it into other behaviors, such as changing color,
	/// selecting a sprite from a list, or adjust scale.
	/// </summary>
	public abstract class Volume : MonoBehaviour
	{
		public AnimationCurve volumeCurve = AnimationCurve.Linear(0, 0, 1, 1);

		public bool preview;
		
		[ShowIf("preview"), Range(0, 1), OnValueChanged("UpdatePreview")]
		public float previewVolume = .5f;

		void UpdatePreview()
		{
			ApplyVolume(previewVolume);
		}
		
		public abstract void ApplyVolume(float newValue);
	}
}