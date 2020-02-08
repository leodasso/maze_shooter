using Arachnid;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class DialogPanel : InteractivePanel
{
	public TalkyText textOutput;
	[Tooltip("Background image (optional) Will be tinted the color of the dialog.")]
	public Image background;
	[Tooltip("Background sprite (optional) Will be tinted the color of the dialog.")]
	public SpriteRenderer spriteBackground;
	Dialog _dialog;
	int _index = 0;

	protected override void Awake()
	{
		base.Awake();
		textOutput.FullClear();
	}

	[Button]
	public void ShowDialog(Dialog dialog)
	{
		textOutput.FullClear();
		ShowPanel();
		_dialog = dialog;
		if (dialog.setColors)
		{
			SetBackgroundColor(dialog.panelColor);
			textOutput.SetTextColor(dialog.textColor);
		}
		textOutput.charactersPerSecond = _dialog.charactersPerSecond;
		_index = 0;
		ShowText(_index);
	}

	void SetBackgroundColor(Color color)
	{
		if (background) 
			background.color = color;
		if (spriteBackground)
			spriteBackground.color = color;
	}

	protected override void OkayButtonPressed()
	{
		base.OkayButtonPressed();
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

		ExitPanel();
	}

	public override void ExitPanel()
	{
		if (_dialog.progressCurrentSequenceWhenComplete)
			EventSequence.AdvanceSequence();
		base.ExitPanel();
	}

	void ShowText(int index)
	{
		Debug.Log("Setting input text to " + _dialog.text[index], textOutput);
		textOutput.inputText = _dialog.text[index];
	}
}
