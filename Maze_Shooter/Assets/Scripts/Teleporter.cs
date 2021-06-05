using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arachnid;

public class Teleporter : MonoBehaviour
{
	public Collection toTeleport;

	public void Teleport()
	{
		toTeleport.GetFirstElement().transform.position = transform.position;
	}
}