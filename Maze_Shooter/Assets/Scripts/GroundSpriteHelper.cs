using UnityEngine;
using Sirenix.OdinInspector;

public class GroundSpriteHelper : MonoBehaviour
{
	public GameObject groundSprite;

	[ButtonGroup]
	void ShowGroundSprite() {
		groundSprite.SetActive(true);
	}

	[ButtonGroup]
	void HideGroundSprite() {
		groundSprite.SetActive(false);
	}
}
