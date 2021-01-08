using UnityEngine;
using TMPro;
using Arachnid;

public class IntValueDisplay : MonoBehaviour
{
	[SerializeField]
	TextMeshProUGUI text;

	[SerializeField]
	IntValue intValue;

	[SerializeField, Tooltip("Refreshes the text every update frame")]
	bool refreshOnUpdate;

    // Start is called before the first frame update
    void Start()
    {
		UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        if (refreshOnUpdate) UpdateText();
    }

	public void UpdateText() 
	{
        text.text = intValue.Value.ToString();
	}
}
