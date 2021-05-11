using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControllable
{
    void ApplyLeftStickInput(Vector2 input);
    void ApplyRightStickInput(Vector2 input);
    void DoActionAlpha();
	void OnPlayerControlEnabled(bool isEnabled);

    string Name();
}
