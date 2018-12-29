using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveFileBubble : MonoBehaviour
{
	public SaveDataAvatar avatar;
	public bool hideIfAlreadyUsed;
	SpriteRenderer _spriteRenderer;

	// Use this for initialization
	void Start ()
	{
		Recalculate();
	}

	public void Recalculate()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_spriteRenderer.sprite = avatar.sprite;
		
		if (hideIfAlreadyUsed)
		{
			if (GameMaster.AvatarIsUsedBySaveFile(avatar))
				gameObject.SetActive(false);
		}
	}

	/// <summary>
	/// Sets my avatar as the current play session's avatar. 
	/// </summary>
	public void SetAsAvatar()
	{
		GameMaster.Get().currentAvatar = avatar;
		ES3.Save<int>("shotsFired", 1, GameMaster.saveFilesDirectory + avatar.name + ".es3");
	}
}
