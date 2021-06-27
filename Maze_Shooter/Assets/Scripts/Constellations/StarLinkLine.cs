using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class StarLinkLine : MonoBehaviour
{
	public StarNode nodeA;
	public StarNode nodeB;

	[Space]
	[SerializeField]
	float inactiveAlpha = .1f;
	[SerializeField]
	float activeAlpha = 1;

	[SerializeField]
	LineRendererStraight lineRendererController;

	[SerializeField]
	LineRenderer lineRenderer;

	// the line will have different alpha depending on if the star has been acquired
	float alphaA;
	float alphaB;
	GradientColorKey[] colorKeys = new GradientColorKey[1] {new GradientColorKey(Color.white, 0)};

	float AlphaForStar(StarNode node) {
		if (!node.myStar) return inactiveAlpha;
		return node.myStar.Value ? activeAlpha : inactiveAlpha;
	}

    // Update is called once per frame
    void Update()
    {
		if (lineRendererController) {
        	if (nodeA) 
				lineRendererController.startPoint = nodeA.transform;

			if (nodeB)
				lineRendererController.endPoint = nodeB.transform;
		}

		if (lineRenderer) {

			// Change the color of each end of the gradient to match whether the star has been acquired or not.
			alphaA = Mathf.Lerp(alphaA, AlphaForStar(nodeA), Time.unscaledDeltaTime * 12);
			alphaB = Mathf.Lerp(alphaB, AlphaForStar(nodeB), Time.unscaledDeltaTime * 12);

			GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2] {
				new GradientAlphaKey(alphaA, 0.2f),	// node A alpha
				new GradientAlphaKey(alphaB, 0.8f), 	// node B alpha
			};

			Gradient newGradient = new Gradient();
			newGradient.SetKeys(colorKeys, alphaKeys);
			lineRenderer.colorGradient = newGradient;
		}
    }
}
