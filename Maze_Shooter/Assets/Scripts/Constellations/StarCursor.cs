using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Arachnid;

public class StarCursor : MonoBehaviour
{
	[SerializeField]
	FloatReference moveSpeed;

	[Tooltip("The size of the cursor's bounds, centered on the parent transform origin.")]
	public Vector3 cursorBoundsSize;

	Vector2 moveInput;
	Rewired.Player _player;

	void OnDrawGizmosSelected()
	{
		if (transform.parent)
		{
			Gizmos.color = Color.magenta;
			Gizmos.DrawWireCube(transform.parent.position, cursorBoundsSize);
		}
	}

    void Start()
    {
		_player = ReInput.players.GetPlayer(0);
    }

    void Update()
    {
		moveInput = new Vector2(_player.GetAxis("moveX"), _player.GetAxis("moveY"));
		transform.Translate(moveInput * Time.unscaledDeltaTime * moveSpeed.Value);

		// clamp cursor to bounds
		transform.localPosition = new Vector3(
			Mathf.Clamp(transform.localPosition.x, -cursorBoundsSize.x/2, cursorBoundsSize.x/2),
			Mathf.Clamp(transform.localPosition.y, -cursorBoundsSize.y/2, cursorBoundsSize.y/2),
			Mathf.Clamp(transform.localPosition.z, -cursorBoundsSize.z/2, cursorBoundsSize.z/2)
		);
    }
}
