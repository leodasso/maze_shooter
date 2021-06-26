using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StarTitle : InteractivePanel
{

    public StarData starData;
    public TextMeshProUGUI textBox;

    public override void ShowPanel()
    {
        textBox.text = starData.title;
        base.ShowPanel();
    }

    protected override void OkayButtonPressed()
    {
        base.OkayButtonPressed();
        ExitPanel();
    }
}
