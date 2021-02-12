using UnityEngine;
using UnityEngine.Events;

public class ActionGui : MonoBehaviour
{
	[SerializeField]
	Animator animator;

	[SerializeField]
	UnityEvent onNotify;

	[SerializeField]
	UnityEvent onDeNotify;

	[SerializeField]
	UnityEvent onShowAction;

	[SerializeField]
	UnityEvent onHideAction;


	public void Notify() 
	{
		animator.SetBool("notify", true);
		onNotify.Invoke();
	}

	public void DeNotify() 
	{
		animator.SetBool("notify", false);
		onDeNotify.Invoke();
	}

	public void ShowAction()
	{
		animator.SetBool("action", true);
		onShowAction.Invoke();
	}

	public void HideAction() 
	{
		animator.SetBool("action", false);
		onHideAction.Invoke();
	}
}
