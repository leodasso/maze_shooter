using System.Collections;
using System.Collections.Generic;
using Arachnid;
using Sirenix.OdinInspector;
using UnityEngine;

public class CharacterPath : MonoBehaviour
{
	[ToggleLeft, Tooltip("Does the path connect start to end point?")]
	public bool looped;
	public List<CharacterPathPoint> pathPoints = new List<CharacterPathPoint>();


	[System.Serializable]
	public class CharacterPathPoint 
	{
		public Vector3 pos;
		public string playmakerEvent;
	}
}