public class SpriteAnimSpeedVolume : Volume
{
    public SpriteAnimationPlayer spriteAnimationPlayer;
    
    public override void ApplyVolume(float newValue)
    {
        if (!spriteAnimationPlayer) return;
        spriteAnimationPlayer.speedMultiplier = volumeCurve.Evaluate(newValue);
    }
}