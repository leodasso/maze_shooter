using System.Collections;
using System.Collections.Generic;
using Arachnid;
using Rewired;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogPanel : MonoBehaviour
{
	public TalkyText textOutput;
	public Image background;
	public TextMeshProUGUI textComponent;
	public GameObject speaker;

	Dialog _dialog;
	int _index = 0;
	Rewired.Player _player;

	void Awake()
	{
		_player = ReInput.players.GetPlayer(0);
	}

	public void ShowDialog(Dialog dialog, GameObject newSpeaker)
	{
		_dialog = dialog;
		speaker = newSpeaker;
		background.color = dialog.panelColor;
		textComponent.color = dialog.textColor;
		textOutput.charactersPerSecond = _dialog.charactersPerSecond;
		_index = 0;
		ShowText(_index);
	}

	void Update()
	{
		if (_player.GetButtonDown("alpha"))
			ProgressText();
	}

	void ProgressText()
	{
		if (!textOutput.FullyShowing)
		{
			textOutput.ShowFull();
			return;
		}
		
		textOutput.Clear();
		_index++;

		if (_index < _dialog.text.Count)
		{
			ShowText(_index);
			return;
		}

		Exit();
	}

	void Exit()
	{
		if (_dialog.progressCurrentSequenceWhenComplete)
			EventSequence.AdvanceSequence();
		
		Destroy(gameObject);
	}

	void ShowText(int index)
	{
		textOutput.inputText = _dialog.text[index];
	}
}
