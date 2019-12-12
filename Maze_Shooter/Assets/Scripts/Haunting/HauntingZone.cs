﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arachnid;
using Sirenix.OdinInspector;

namespace ShootyGhost
{
    [TypeInfoBox("The area in which things can be haunted. Controls a selector, which determines which object the " +
                 "player wishes to posess. A 'Player' component is needed on this object to take input from the player.")]
    public class HauntingZone : MonoBehaviour, IControllable
    {
        public FloatReference hauntingRange;
        [Tooltip("The visual representation of the haunting area. This is scalled according to the haunting range.")]
        public GameObject hauntingArea;
        public Transform selector;

        // Update is called once per frame
        void Update()
        {
            hauntingArea.transform.localScale = Vector3.one * hauntingRange.Value;
        }

        public void ApplyRightStickInput(Vector2 input)
        {
            // Move the selector based on right stick movement
            // Sometimes the input's magnitude can exceed 1, so this ensures it stays within the range.
            Vector3 totalInput = new Vector3(input.x, 0, input.y);
            Vector3 clampedInput = Vector3.ClampMagnitude(totalInput, 1);
            selector.transform.localPosition = clampedInput * hauntingRange.Value;
        }

        public void DoActionAlpha() { }

        public void ApplyLeftStickInput(Vector2 input) { }

        public string Name()
        {
            return "haunter " + name;
        }
    }
}