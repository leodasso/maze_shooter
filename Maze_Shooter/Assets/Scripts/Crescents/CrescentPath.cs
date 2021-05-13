using UnityEngine;
using Cinemachine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(CinemachinePath))]
public class CrescentPath : MonoBehaviour
{
	[SerializeField]
	CinemachinePath path;

	[Tooltip("Determines how high a crescent goes when activated")]
	public float height = 10;

	[Tooltip("Determines how long the anchor point is for the path point going into the glyph")]
	public float glyphEntryStrength = 15;

	[Button]
	public void SetupPath(Crescent crescent, CrescentGlyph glyph)
	{
		path.m_Waypoints[0].position = path.transform.InverseTransformPoint(crescent.transform.position);
		path.m_Waypoints[0].tangent = Vector3.up * height;

		path.m_Waypoints[1].position = path.transform.InverseTransformPoint(glyph.transform.position);
		path.m_Waypoints[1].tangent = -glyph.facingDirection.normalized * glyphEntryStrength;
	}
}
