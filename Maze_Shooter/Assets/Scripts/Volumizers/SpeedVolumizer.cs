using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShootyGhost {

	public class SpeedVolumizer : Volumizer
	{
		public PseudoVelocity pseudoVelocity;
		[Tooltip("X axis is speed, y axis is volume")]
		public AnimationCurve speedToVolume = AnimationCurve.Linear(0, 0, 5, 1);

		float _speed;

		// Update is called once per frame
		void Update()
		{
			_speed = pseudoVelocity.velocity.magnitude;
			UpdateNormalizedValue(speedToVolume.Evaluate(_speed));
			UpdateVolumes();
		}
	}

}