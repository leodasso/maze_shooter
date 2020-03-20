public class ColorGroupVolume : Volume
{
    public ColorGroup colorGroup;
    
    public override void ApplyVolume(float newValue)
    {
        if (!colorGroup) return;
        colorGroup.alpha = volumeCurve.Evaluate(newValue);
    }
}