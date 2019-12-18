using Arachnid;
using Rewired;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class InteractivePanel : MonoBehaviour
{
	public bool destroyWhenComplete = true;
	[Tooltip("Is this dialog panel currently showing?")]
	public bool active;

	public UnityEvent onPanelStart;
	public UnityEvent onPanelComplete;
	public UnityEvent onOkayButtonPressed;
	[Tooltip("Optional - will set 'visible' bool in animator when shown or hidden")]
	public Animator animator;

	Rewired.Player _player;

	void Awake()
	{
		_player = ReInput.players.GetPlayer(0);
	}

	public virtual void ShowPanel()
	{
		active = true;
		onPanelStart.Invoke();
		animator?.SetBool("visible", true);
	}

	void Update()
	{
		if (_player.GetButtonDown("alpha") && active)
			OkayButtonPressed();
	}

	protected virtual void OkayButtonPressed()
	{
		onOkayButtonPressed.Invoke();
	}

	/// <summary>
	/// Exits the panel, calling relevant Unity Events. If you're inheriting from this,
	/// the base.ExitPanel() should most likely be after all the inheriting code, because
	/// this function may destroy the object.
	/// </summary>
	protected virtual void ExitPanel()
	{
		active = false;		
		onPanelComplete.Invoke();
		animator?.SetBool("visible", false);
		if (destroyWhenComplete)
			Destroy(gameObject);
	}
}
