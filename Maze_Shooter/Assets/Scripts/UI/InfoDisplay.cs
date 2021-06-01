using UnityEngine;

public abstract class InfoDisplay : MonoBehaviour
{
	[SerializeField, Tooltip("Refreshes the display every update frame")]
	bool refreshOnUpdate;

    // Start is called before the first frame update
    void Start()
    {
		UpdateDisplay();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (refreshOnUpdate) UpdateDisplay();
    }

	public abstract void UpdateDisplay();
}