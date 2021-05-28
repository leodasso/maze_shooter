using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AnimatorTools : MonoBehaviour
{
	[MinMaxSlider(0, 4, true)]
	public Vector2 playbackSpeed = new Vector2(1, 1);

	public float randomOffset = 1;

	public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator.speed = Random.Range(playbackSpeed.x, playbackSpeed.y);
		animator.Play(0, -1, Random.Range(0, randomOffset));
    }
}