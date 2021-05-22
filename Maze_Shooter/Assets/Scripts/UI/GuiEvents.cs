using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Rewired;
using Sirenix.OdinInspector;

[TypeInfoBox("This component listens for player inputs and calls events. " +
"Example: If player presses the 'pause' button, the pause event will be called.")]
public class GuiEvents : MonoBehaviour
{
	[SerializeField]
	UnityEvent pause;

	[SerializeField]
	UnityEvent accept;

	[SerializeField]
	UnityEvent back;

	Rewired.Player _player;

    // Start is called before the first frame update
    void Start()
    {
		_player = ReInput.players.GetPlayer(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.GetButtonUp("pause")) pause.Invoke();
		if (_player.GetButtonUp("gui_accept")) accept.Invoke();
        if (_player.GetButtonUp("gui_back")) back.Invoke();
    }
}
