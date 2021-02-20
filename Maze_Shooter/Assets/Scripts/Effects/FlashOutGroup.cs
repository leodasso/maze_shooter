using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class FlashOutGroup : MonoBehaviour
{
	[SerializeField]
	List<FlashOut> flashOuters = new List<FlashOut>();

	public void DoFlashAll()
	{
		foreach(var flasher in flashOuters)
			flasher.DoFlash();
	}

	[Button]
	public void GetFlashOuters()
	{
		flashOuters.Clear();
		flashOuters.AddRange(GetComponentsInChildren<FlashOut>());
	}
}
