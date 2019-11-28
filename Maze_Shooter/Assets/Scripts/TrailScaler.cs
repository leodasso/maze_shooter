using UnityEngine;

[ExecuteAlways]
public class TrailScaler : MonoBehaviour
{
    public TrailRenderer trailRenderer;
    public float scaleMultiplier = 1;
    
    // Update is called once per frame
    void Update()
    {
        if (!trailRenderer) return;
        trailRenderer.widthMultiplier = transform.lossyScale.x * scaleMultiplier;
    }
}
