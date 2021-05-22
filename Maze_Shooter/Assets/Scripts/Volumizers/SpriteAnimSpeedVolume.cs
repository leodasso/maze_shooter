public class SpriteAnimSpeedVolume : Volume
{
    public ShootyGhost.SpriteAnimationPlayer spriteAnimationPlayer;
    
    public override void ApplyVolume(float newValue)
    {
        if (!spriteAnimationPlayer) return;
        spriteAnimationPlayer.speedMultiplier = volumeCurve.Evaluate(newValue);
    }
}