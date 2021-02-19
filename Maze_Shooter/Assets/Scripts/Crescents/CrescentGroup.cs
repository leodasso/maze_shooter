using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Arachnid;
using Sirenix.OdinInspector;

public class CrescentGroup : MonoBehaviour
{
	public Collection crescentCollection;
	public List<CrescentGlyph> crescentGlyphs = new List<CrescentGlyph>();

	public UnityEvent onActivated;

	public UnityEvent onComplete;

	static HashSet<CrescentGroup> crescentGroups = new HashSet<CrescentGroup>();

	List<CrescentGlyph> availableGlyphs = new List<CrescentGlyph>();
	HashSet<CrescentGlyph> activatedGlyphs = new HashSet<CrescentGlyph>();

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

	public void ActivateGlyph(CrescentGlyph glyph) 
	{
		activatedGlyphs.Add(glyph);
		glyph.Activate();

		if (activatedGlyphs.Count >= crescentGlyphs.Count)
			onComplete.Invoke();
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