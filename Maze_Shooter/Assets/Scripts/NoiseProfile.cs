using UnityEngine;

[CreateAssetMenu(menuName = "Ghost/noise profile")]
public class NoiseProfile : ScriptableObject
{
    public float frequency = 1;
    public float strength = 1;
    public Vector2 scrollSpeed = Vector2.right;
}