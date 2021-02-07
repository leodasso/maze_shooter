using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewFileGui : MonoBehaviour
{
	[SerializeField]
	SaveDataAvatar avatar;

	[SerializeField]
	TextMeshProUGUI text;

	[SerializeField]
	Image sprite;

	[SerializeField]
	Button button;

	[SerializeField]
	bool disableIfUsed;


    // Start is called before the first frame update
    void Start()
    {
		if (avatar)
        	Recalculate();
    }

	public void SetAvatar(SaveDataAvatar newAvatar) 
	{
		avatar = newAvatar;
		Recalculate();
	}

	void Recalculate()
	{
		if (!avatar) return;
		text.text = avatar.name;

		sprite.sprite = avatar.sprite;
		
		if (disableIfUsed)
			button.interactable = !GameMaster.AvatarIsUsedBySaveFile(avatar);
	}

	/// <summary>
	/// Sets my avatar as the current play session's avatar. 
	/// </summary>
	public void NewGameWithThis()
	{
		GameMaster.Get().currentAvatar = avatar;
		SelectThisFile();
	}

	public void LoadThis() 
	{
		GameMaster.Get().currentAvatar = avatar;
		SelectThisFile();
	}

	void SelectThisFile() 
	{
		SaveFileMenu parentMenu = GetComponentInParent<SaveFileMenu>();
		if (!parentMenu) {
			Debug.LogError("No load menu component found in heirarchy! can't load.");
			return;
		}

		parentMenu.SelectFile(avatar);
	}
}