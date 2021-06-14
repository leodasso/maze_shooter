using UnityEditor;
using UnityEngine;

namespace Arachnid
{
	[CustomPropertyDrawer(typeof(ObjectReference))]
	public class ObjectRefDrawer : ValueRefDrawer<ObjectValue>
	{	}
}