using Arachnid;
using Rewired;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class InteractivePanel : MonoBehaviour
{
	public bool destroyWhenComplete = true;
	[Tooltip("Is this dialog panel currently showing?")]
	public bool active;

	[Tooltip("Is this a UI element that requires a parent canvas?")]
	public bool requireCanvas;

	[ShowIf("requireCanvas"), Tooltip("Choose which type of canvas is required as a parent." +
	                                  " If the canvas doesn't already exist in the scene, it will be " +
	                                  "instantiated for you.")]
	public GuiCanvasType canvasType;

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

	/// <summary>
	/// Use this rather than instantiate for UI panels. This function determines
	/// if a UI canvas is needed for this prefab, and instantiates it if needed.
	/// </summary>
	public InteractivePanel CreateInstance()
	{
		GameObject newInstance = Instantiate(gameObject, ParentCanvas());
		return newInstance.GetComponent<InteractivePanel>();
	}

	Transform ParentCanvas()
	{
		if (!requireCanvas) return null;
		if (!canvasType)
		{
			Debug.LogError(gameObject.name + " requires a canvas to be instantiated, but no canvas type is defined!");
			return null;
		}
		return canvasType.GetInstance().transform;
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
	public virtual void ExitPanel()
	{
		active = false;		
		onPanelComplete.Invoke();
		animator?.SetBool("visible", false);
		if (destroyWhenComplete)
			Destroy(gameObject);
	}
}
