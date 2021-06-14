using UnityEngine;
using Arachnid;
using UnityEditor;

[CustomPropertyDrawer(typeof(HeartsRef))]
public class HeartsRefDrawer : ValueRefDrawer<HeartsValue>
{
	protected override float ObjRefWidth => .4f;
}