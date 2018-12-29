using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using UnityEngine.Timeline;

public class Plant : MonoBehaviour, IWettable
{
	[Range(0, 1)]
	public float development = .5f;

	[Range(-1, 1)]
	public float hydration = 0;

	[Tooltip("When this plant is watered, the quantity of water is multiplied by this to add to hydration")]
	public FloatReference hydrationRatio;
	public FloatReference drain;
	
	[Tooltip("Y axis is how much it will grow, X axis is how far along development it is")]
	public AnimationCurve growthOverDevelopment;
	
	[Space]
	public SpriteRenderer cactusSprite;
	public Gradient hydrationColors;
	public Transform cactusScale;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (hydration > -1)
			hydration -= drain.Value * Time.deltaTime;

		else
			development -= drain.Value * Time.deltaTime;
		
		cactusScale.localScale = new Vector3(1, development, 1);
		float gradientEvaluate = (hydration + 1) / 2;
		cactusSprite.color = hydrationColors.Evaluate(gradientEvaluate);
	}
	


	public void AddWater(float howMuch)
	{
		development += growthOverDevelopment.Evaluate(development) * howMuch;
		hydration += hydrationRatio.Value * howMuch;
	}
	
}
