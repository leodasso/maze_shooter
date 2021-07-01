using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ShootyGhost {

	public class SelectionCaster : MonoBehaviour
	{
		public Selectable selected;

		[SerializeField]
		public Transform cursor;

		[SerializeField]
		Cinemachine.CinemachineVirtualCamera cam;

		[SerializeField]
		LayerMask castingMask;

		List<Selectable> hoveredSelectables = new List<Selectable>();


		// Start is called before the first frame update
		void Start()
		{
		}

		// Update is called once per frame
		void Update()
		{
			hoveredSelectables.Clear();

			Vector3 castingDir = cursor.position - cam.transform.position;

			Debug.DrawRay(cam.transform.position, castingDir, Color.red, 10);
			
			foreach (var hit in Physics.RaycastAll(cam.transform.position, castingDir.normalized, 1000, castingMask)) {

				Selectable selectable = hit.collider.GetComponent<Selectable>();
				if (!selectable) continue;
				if (!selectable.enabled) continue;
				hoveredSelectables.Add(selectable);
			}

			hoveredSelectables = hoveredSelectables.OrderBy(x => Vector3.Distance(x.transform.position, cursor.position)).ToList();

			var prevSelected = selected;

			if (hoveredSelectables.Count > 0) 
				selected = hoveredSelectables[0];
			else selected = null;

			// call events on selectables
			if (selected != prevSelected) {
				if ( prevSelected != null)
					prevSelected.HoverExit();

				if (selected != null)
					selected.HoverEnter();
			}

			cam.Follow = selected ? selected.transform : cursor;
		}
	}
}