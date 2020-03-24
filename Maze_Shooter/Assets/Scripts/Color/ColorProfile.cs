using UnityEngine;

[CreateAssetMenu(menuName = "Ghost/Color Profile")]
public class ColorProfile : ScriptableObject
{
    public Color primary = Color.white;
    public Color secondary = Color.cyan;
    public Color tertiary = Color.yellow;

    public Color GetColor(ColorCategory category)
    {
        if (category == ColorCategory.Secondary) return secondary;
        if (category == ColorCategory.Tertiary) return tertiary;
        return primary;
    }
}

public enum ColorCategory
{
    Primary,
    Secondary,
    Tertiary,
}
