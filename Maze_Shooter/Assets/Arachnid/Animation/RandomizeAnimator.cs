using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Arachnid
{
    [RequireComponent(typeof(Animator))]
    public class RandomizeAnimator : MonoBehaviour
    {
        [Tooltip("The animator's playback speed will be set to a random range between these values on awake.")]
        [MinMaxSlider(0.01f, 10, true)]
        public Vector2 randomSpeedRange = Vector2.one;
        
        // Start is called before the first frame update
        void Awake()
        {
            Animator animator = GetComponent<Animator>();
            if (!animator) return;
            animator.speed = Random.Range(randomSpeedRange.x, randomSpeedRange.y);
        }
    }
}