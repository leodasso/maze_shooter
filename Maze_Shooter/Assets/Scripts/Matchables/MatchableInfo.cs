using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu]
public class MatchableInfo : ScriptableObject
{
	[AssetsOnly]
	public List<GameObject> evolutions = new List<GameObject>();
}
