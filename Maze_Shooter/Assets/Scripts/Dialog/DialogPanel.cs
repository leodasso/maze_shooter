using Arachnid;
using Rewired;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class DialogPanel : MonoBehaviour
{
	public TalkyText textOutput;
	[Tooltip("Background image (optional) Will be tinted the color of the dialog.")]
	public Image background;
	[Tooltip("Background sprite (optional) Will be tinted the color of the dialog.")]
	public SpriteRenderer spriteBackground;
	public bool destroyWhenComplete = true;
	[Tooltip("Is this dialog panel currently showing dialog?")]
	public bool active;

	public UnityEvent onDialogStart;
	public UnityEvent onDialogComplete;
	[Tooltip("Optional - will set 'visible' bool in animator when shown or hidden")]
	public Animator animator;

	Dialog _dialog;
	int _index = 0;
	Rewired.Player _player;

	void Awake()
	{
		_player = ReInput.players.GetPlayer(0);
	}

	void Start()
	{
		textOutput.FullClear();
	}

	[Button]
	public void ShowDialog(Dialog dialog)
	{
		active = true;
		textOutput.FullClear();
		onDialogStart.Invoke();
		_dialog = dialog;
		if (dialog.setColors)
		{
			SetBackgroundColor(dialog.panelColor);
			textOutput.SetTextColor(dialog.textColor);
		}
		textOutput.charactersPerSecond = _dialog.charactersPerSecond;
		_index = 0;
		animator?.SetBool("visible", true);
		ShowText(_index);
	}

	void SetBackgroundColor(Color color)
	{
		if (background) 
			background.color = color;
		if (spriteBackground)
			spriteBackground.color = color;
	}

	void Update()
	{
		if (_player.GetButtonDown("alpha") && active)
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
		active = false;
		if (_dialog.progressCurrentSequenceWhenComplete)
			EventSequence.AdvanceSequence();
		
		onDialogComplete.Invoke();
		animator?.SetBool("visible", false);
		if (destroyWhenComplete)
			Destroy(gameObject);
	}

	void ShowText(int index)
	{
		Debug.Log("Setting input text to " + _dialog.text[index]);
		textOutput.inputText = _dialog.text[index];
	}
}
