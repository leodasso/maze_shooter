using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ColorGroup : MonoBehaviour
{
    [OnValueChanged("SetColor")]
    public Color color = Color.white;
    public bool overwriteAlpha;
    public List<SpriteRenderer> sprites = new List<SpriteRenderer>();
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetColor()
    {
        GetSprites();
        foreach (var sprite in sprites)
        {
            sprite.color = new Color(color.r, color.g, color.b, sprite.color.a);
        }
    }

    [Button]
    public void GetSprites()
    {
        sprites.Clear();
        sprites.AddRange(GetComponentsInChildren<SpriteRenderer>());
    }
}
