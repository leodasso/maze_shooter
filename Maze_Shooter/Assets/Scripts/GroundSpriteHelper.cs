using UnityEngine;
using Sirenix.OdinInspector;

public class GroundSpriteHelper : MonoBehaviour
{
	public GameObject groundSprite;

	[ButtonGroup]
	void ShowGroundSprite() {
		groundSprite.SetActive(true);
		groundSprite.GetComponent<SpriteRenderer>().color = Color.white;
	}

	[ButtonGroup]
	void HideGroundSprite() {
		groundSprite.SetActive(true);
		groundSprite.GetComponent<SpriteRenderer>().color = Color.black;
	}
}
