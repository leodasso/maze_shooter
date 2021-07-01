using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ShootyGhost {

	public class Selectable : MonoBehaviour
	{
		[SerializeField]
		UnityEvent onHoverEnter;

		[SerializeField]
		UnityEvent onHoverExit;

		[SerializeField]
		UnityEvent onSelect;

		// Start is called before the first frame update
		void Start()
		{
			
		}

		public void HoverEnter()
		{
			onHoverEnter.Invoke();
		}

		public void HoverExit()
		{
			onHoverExit.Invoke();
		}
	}
}