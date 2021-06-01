using UnityEngine;
using TMPro;
using Arachnid;

public class IntValueDisplay : InfoDisplay
{
	[SerializeField]
	TextMeshProUGUI text;

	[SerializeField]
	IntValue intValue;

	public override void UpdateDisplay() 
	{
        text.text = intValue.Value.ToString();
	}
}