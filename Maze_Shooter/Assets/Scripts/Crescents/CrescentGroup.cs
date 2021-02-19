using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arachnid;
using Sirenix.OdinInspector;

public class CrescentGroup : MonoBehaviour
{
	public Collection crescentCollection;
	public List<CrescentGlyph> crescentGlyphs = new List<CrescentGlyph>();

	static HashSet<CrescentGroup> crescentGroups = new HashSet<CrescentGroup>();

	List<CrescentGlyph> availableGlyphs = new List<CrescentGlyph>();

	public static CrescentGroup CrescentGroupForCollection(Collection collection) 
	{
		foreach( CrescentGroup g in crescentGroups) {
			if (g.crescentCollection == collection) return g;
		}
		return null;
	}

    void Awake()
    {
        crescentGroups.Add(this);
		availableGlyphs = new List<CrescentGlyph>(crescentGlyphs);
    }

	void OnDestroy()
	{
		crescentGroups.Remove(this);
	}

	public CrescentGlyph GetEmptyGlyph() 
	{
		CrescentGlyph emptyGlyph = availableGlyphs[0];
		availableGlyphs.RemoveAt(0);
		return emptyGlyph;
	}

	[Button]
	void GetCrescentGlyphsInChildren() 
	{
		crescentGlyphs.Clear();
		crescentGlyphs.AddRange(GetComponentsInChildren<CrescentGlyph>());
	}
}