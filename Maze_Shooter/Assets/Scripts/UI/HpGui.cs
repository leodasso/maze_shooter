using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Arachnid;

public class HpGui : MonoBehaviour
{
	public IntValue currentHp;
	public IntValue maxHp;
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
			heart.gameObject.SetActive(i < maxHp.Value);
			if (heart.isActiveAndEnabled)
				heart.filled = i < currentHp.Value;
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
