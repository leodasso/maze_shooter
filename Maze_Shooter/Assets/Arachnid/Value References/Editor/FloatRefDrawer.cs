using UnityEditor;

namespace Arachnid
{
	[CustomPropertyDrawer(typeof(FloatReference))]
	public class FloatRefDrawer : ValueRefDrawer<FloatValue>
	{	}
}