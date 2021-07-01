using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Arachnid;

public class StarCursor : MonoBehaviour
{
	public FloatReference moveSpeed;
	Vector2 moveInput;
	Rewired.Player _player;

    void Start()
    {
		_player = ReInput.players.GetPlayer(0);
    }

    void Update()
    {
		moveInput = new Vector2(_player.GetAxis("moveX"), _player.GetAxis("moveY"));
		transform.Translate(moveInput * Time.unscaledDeltaTime * moveSpeed.Value);
    }
}
