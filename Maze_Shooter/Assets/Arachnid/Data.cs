using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arachnid
{
	public enum PropertyType { Local, Global }
	public enum TargetType { Nearest, Farthest, Random}

	public static class EditorGlobals
	{
		public static float propTypeEnumWidth = 50;
	}
}