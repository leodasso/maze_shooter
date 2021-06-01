using UnityEngine;
using UnityEngine.UI;
using Arachnid;

public class FloatValueToFill : InfoDisplay
{
	[SerializeField]
	Image image;

	[SerializeField]
	FloatReference value;

	[SerializeField]
	FloatReference maxValue;

	[SerializeField]
	AnimationCurve valueToFill = AnimationCurve.Linear(0, 0, 1, 1);

	[SerializeField]
	float lerpSpeed = 5;

	float actualValue;

	protected override void Update()
	{
		image.fillAmount = Mathf.Lerp(image.fillAmount, actualValue, Time.unscaledDeltaTime * lerpSpeed);
		base.Update();
	}

	public override void UpdateDisplay()
	{
		float normalized = value.Value / maxValue.Value;
		actualValue = valueToFill.Evaluate(normalized);
	}
}
