using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class HpGui : MonoBehaviour
{
	[SerializeField, MinValue(0.1f)]
	float flashesPerSecond = 1.5f;

	[Space]
	public HeartsValue currentHp;
	public HeartsValue maxHp;

	Hearts currentHearts;
	Hearts prevHearts;

	public List<GuiHeart> hearts = new List<GuiHeart>();

	float flashDuration => 1 / flashesPerSecond;

    // Start is called before the first frame update
    void Start()
    {
		FetchNewValue();
        Refresh(currentHearts);
    }

	public void FetchNewValue() 
	{
		if (currentHearts == currentHp.Value) return;
		prevHearts = currentHearts;
		currentHearts = currentHp.Value;

		Debug.Log("Fetched new value. Prev hearts and current hearts: " + prevHearts + " " + currentHearts);
	}

    public void Refresh(Hearts displayedValue) 
	{
		for (int i = 0; i < hearts.Count; i++) 
		{
			var heart = hearts[i];
			heart.gameObject.SetActive(i < maxHp.Value.hearts);
			if (!heart.isActiveAndEnabled) continue;

			// if this is the last heart, show fractions
			if (i == displayedValue.hearts && displayedValue.fractions > 0) 
				heart.fraction = displayedValue.fractions;

			// otherwise, show full / empty hearts
			else heart.filled = i < displayedValue.hearts;
		}
	}

	public void ShowCurrentHearts()
	{
		EndFlash();
		Refresh(currentHearts);
	}

	public void BeginFlash() 
	{
		StartCoroutine(DoFlash());
	}

	public void EndFlash()
	{
		StopAllCoroutines();
	}

	IEnumerator DoFlash()
	{
		Debug.Log("Flashing between " + prevHearts + " and " + currentHearts);
		while (true) {
			Refresh(prevHearts);
			yield return new WaitForSecondsRealtime(flashDuration / 2);

			Refresh(currentHearts);
			yield return new WaitForSecondsRealtime(flashDuration / 2);
		}
	}
}