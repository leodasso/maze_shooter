using UnityEngine;
using Sirenix.OdinInspector;

[TypeInfoBox("Sets the progress of the sprite animation based on the normalized health")]
public class HealthToSpriteAnimator : MonoBehaviour
{
	public bool reverse;
	public Health health;
	public SpriteAnimator spriteAnimator;

	public float newProgress;
	

    // Update is called once per frame
    void Update()
    {
		newProgress = reverse ? 1 - health.NormalizedHp : health.NormalizedHp;
        spriteAnimator.SetProgress(newProgress);
    }
}