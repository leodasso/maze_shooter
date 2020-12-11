using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HauntStar : MonoBehaviour
{
	public float progress;
	public HauntStarSlot slot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void GotoSlot(HauntStarSlot newSlot, float duration) 
	{
		slot = newSlot;
	}

	IEnumerator PlayAnimSequence(float beginningValue, float endValue, float duration) 
	{
		float lerp = 0;
		while (lerp < 1) {
			
			progress = Mathf.Lerp(beginningValue, endValue, lerp);
			lerp += Time.unscaledDeltaTime / duration;
			yield return null;
		}
		progress = endValue;
	}
}
