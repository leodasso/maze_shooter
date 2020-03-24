using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Ghost/Dialog")]
public class Dialog : ScriptableObject
{
    [MultiLineProperty(6), HideLabel]
    public List<string> text = new List<string>();

    [Tooltip("Set custom background and text colors on whatever panel shows this dialog")]
    public bool setColors = true;
    [ShowIf("setColors")]
    public Color panelColor = new Color(.15f, .15f, .15f);
    [ShowIf("setColors")]
    public Color textColor = new Color(.95f, .95f, .95f);
    [AssetsOnly]
    public DialogPanel panelPrefab;
    public int charactersPerSecond = 50;

    [ToggleLeft]
    public bool progressCurrentSequenceWhenComplete = true;

    public void Display()
    {
        if (!panelPrefab)
        {
            Debug.LogError(name + " has no dialog prefab set, so can't display dialog!", this);
            return;
        }

        var panelInstance = panelPrefab.CreateInstance() as DialogPanel;
        panelInstance.ShowDialog(this);
    }
}
