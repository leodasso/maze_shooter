using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu]
public class Dialog : ScriptableObject
{
    [MultiLineProperty(6), HideLabel]
    public List<string> text = new List<string>();
    public Color panelColor = new Color(.15f, .15f, .15f);
    public Color textColor = new Color(.95f, .95f, .95f);
    [AssetsOnly]
    public GameObject panelPrefab;
    public int charactersPerSecond = 50;

    [ToggleLeft]
    public bool progressCurrentSequenceWhenComplete = true;

    public void Display(GameObject speaker)
    {
        if (!panelPrefab)
        {
            Debug.LogError(name + " has no dialog prefab set, so can't display dialog!", this);
            return;
        }

        GameObject instance = Instantiate(panelPrefab);
        DialogPanel p = instance.GetComponent<DialogPanel>();
        p.ShowDialog(this, speaker);
    }
}
