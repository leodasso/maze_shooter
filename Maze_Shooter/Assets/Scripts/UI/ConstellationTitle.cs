using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConstellationTitle : InteractivePanel
{
    public ConstellationData constellationData;
    public TextMeshProUGUI textBox;

    public override void ShowPanel()
    {
        textBox.text = constellationData.title;
        base.ShowPanel();
    }

    protected override void OkayButtonPressed()
    {
        base.OkayButtonPressed();
        ExitPanel();
    }
}
