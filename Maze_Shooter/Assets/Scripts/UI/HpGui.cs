using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Arachnid;

public class HpGui : MonoBehaviour
{
	public HeartsValue currentHp;
	public HeartsValue maxHp;
	public List<GuiHeart> hearts = new List<GuiHeart>();

    // Start is called before the first frame update
    void Start()
    {
        Refresh();
    }

    public void Refresh() 
	{
		for (int i = 0; i < hearts.Count; i++) 
		{
			var heart = hearts[i];
			heart.gameObject.SetActive(i < maxHp.Value.hearts);
			if (!heart.isActiveAndEnabled) continue;

			// if this is the last heart, show fractions
			if (i == currentHp.Value.hearts && currentHp.Value.fractions > 0) 
				heart.fraction = currentHp.Value.fractions;

			// otherwise, show full / empty hearts
			else heart.filled = i < currentHp.Value.hearts;
		}
	}

	public void DelayRefresh() 
	{
		StartCoroutine(DelayAndRefresh());
	}

	IEnumerator DelayAndRefresh() {
		yield return new WaitForSecondsRealtime(.5f);
		Refresh();
	}
}
