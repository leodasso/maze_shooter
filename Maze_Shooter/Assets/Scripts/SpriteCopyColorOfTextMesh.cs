using System.Collections;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

[ExecuteAlways]
public class SpriteCopyColorOfTextMesh : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public enum tmpType { Normal, UGUI}

    public tmpType type = tmpType.Normal;

    [ShowIf("isNormalTmp")]
    public TextMeshPro textMesh;
    [HideIf("isNormalTmp")]
    public TextMeshProUGUI textMeshUgui;

    Color _color;

    [Button]
    void GetSpriteRenderer()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    bool isNormalTmp => type == tmpType.Normal;

    // Update is called once per frame
    void Update()
    {
        if (!spriteRenderer) return;
        if (isNormalTmp && !textMesh) return;
        if (!isNormalTmp && !textMeshUgui) return;

        _color = isNormalTmp ? textMesh.color : textMeshUgui.color;

        spriteRenderer.color = _color;
    }
}
