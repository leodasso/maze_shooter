using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class FlashOut : MonoBehaviour
{
	[Tooltip("In seconds, a random wait time will be chosen between these 2 values.")]
	[MinMaxSlider(0, 30, true)]
	public Vector2 minMaxTime = new Vector2(1, 2);

	public float flashInterval = .1f;
	public int flashes = 3;

	[Space, ToggleLeft, Tooltip("Disables this object when the flashing is complete")]
	public bool disableOnComplete = true;

	[SerializeField]
	SpriteRenderer spriteRenderer;

	[SerializeField]
	Material invertMaterial;

	Material defaultMaterial;

    // Start is called before the first frame update
    void Start()
    {
        defaultMaterial = spriteRenderer.material;
    }


	[Button]
	public void TestFlash() 
	{
		if (spriteRenderer == null) return;
		StartCoroutine(FlashOutSequence(0));
	}

	public void DoFlash()
	{
		if (spriteRenderer == null) return;
		StartCoroutine(FlashOutSequence(Random.Range(minMaxTime.x, minMaxTime.y)));
	}

	IEnumerator FlashOutSequence(float waitTime) 
	{
		yield return new WaitForSeconds(waitTime);

		int flashesDone = 0;
		while (flashesDone < flashes) 
		{
			// invert material
			spriteRenderer.material = invertMaterial;
			yield return new WaitForSeconds(flashInterval / 2);

			// return to default material
			spriteRenderer.material = defaultMaterial;
			yield return new WaitForSeconds(flashInterval / 2);
			flashesDone++;
		}

		if (disableOnComplete)
			gameObject.SetActive(false);
	}
}
