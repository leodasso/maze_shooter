using UnityEditor;

namespace Arachnid
{
	[CustomPropertyDrawer(typeof(IntReference))]
	public class IntRefDrawer : ValueRefDrawer<IntValue>
	{	}
}