using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CrystalChest : MonoBehaviour, IControllable
{
	[ReadOnly, ShowInInspector]
	bool isHaunted;

	[ReadOnly, ShowInInspector]
	bool isOpen;
	public SpriteRenderer spriteRenderer;

	public Sprite normalSprite;
	public Sprite emptySprite;

	[Space]
	public SpriteAnimator haunted;
	public SpriteAnimator hauntedLeft;
	public SpriteAnimator hauntedRight;

	[Space, Tooltip("Player shakes joystick back and forth, how much damage per each clack")]
	public Hearts damagePerShake;
	public Health health;

	List<SpriteAnimator> animators = new List<SpriteAnimator>();

	Vector2 _input;

	Sprite idleSprite => isOpen ? emptySprite : normalSprite;


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer.sprite = normalSprite;
		animators.Add(haunted);
		animators.Add(hauntedLeft);
		animators.Add(hauntedRight);

		SetAnimation(null);
    }

    // Update is called once per frame
    void Update()
    {
		if (isHaunted) {
			if (_input.x < -.5f)
				SetAnimation(hauntedLeft);
			else if (_input.x > .5f)
				SetAnimation(hauntedRight);
			else SetAnimation(haunted);
		}
		else 		
			spriteRenderer.sprite = idleSprite;
    }

	void SetAnimation(SpriteAnimator anim)
	{
		for (int i = 0; i < animators.Count; i++) 
			animators[i].enabled = animators[i] == anim;
	}

	// below functions intentionally have no arguments so they can be easily called by playmaker
	[ButtonGroup]
	public void SetHaunted() 
	{
		isHaunted = true;
	}

	[ButtonGroup]
	public void SetUnHaunted()
	{
		isHaunted = false;
		SetAnimation(null);
		spriteRenderer.sprite = idleSprite;
	}

	public void DoDamage()
	{
		health.DoDamage(damagePerShake);
	}

	public void SetOpen(bool willBeOpen) 
	{
		isOpen = willBeOpen;
	}

	public void ShowEmpty()
	{
		SetAnimation(null);
		spriteRenderer.sprite = idleSprite;
	}

	public void OnPlayerControlEnabled(bool isEnabled)
	{
		if (!isEnabled) _input = Vector2.zero;
	}

	public void ApplyLeftStickInput(Vector2 input) 
	{
		_input = input;
	}

	public void ApplyRightStickInput(Vector2 input) 
	{
	}

	public void DoActionAlpha() 
	{
	}

	public string Name() 
	{
		return "Crystal Chest " + name;
	}
}
