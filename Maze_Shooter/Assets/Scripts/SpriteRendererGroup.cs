using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[ExecuteAlways]
public class SpriteRendererGroup : MonoBehaviour
{
    [Tooltip("The main sprite renderer that controls properties of the others")]
    public SpriteRenderer master;

    [Range(0, 1)]
    public float alpha = 1;
    
    public List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

    [Button]
    void FindMasterSpriteRenderer()
    {
        master = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!master) return;
        master.color = new Color(master.color.r, master.color.g, master.color.b, alpha);
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.color = master.color;
        }
    }
}
