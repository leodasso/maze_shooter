using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arachnid.Tests
{
	public class SpiralTest : MonoBehaviour
	{
		public int numPoints = 500;
		public float theta = .1f;
		public float A = 1;
		public int startOffset = 2;

		void OnDrawGizmos()
		{
			for (int i = startOffset; i < numPoints; i++)
			{
				Vector2 coords = Math.PointOnSpiral((float)i * theta, A);
				Gizmos.DrawWireSphere(transform.TransformPoint(Math.Project2Dto3D(coords)), .25f);
			}
		}
	}
}