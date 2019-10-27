using UnityEngine;
using Sirenix.OdinInspector;

[ExecuteAlways]
public class LineRendererStraight : MonoBehaviour
{
    [InfoBox("Line Renderer should be set to use World Space for this component to work correctly", 
        InfoMessageType.Warning, "showLineRendererWarning")]
    public LineRenderer lineRenderer;

    public Transform startPoint;
    public Transform endPoint;

    public bool showLineRendererWarning()
    {
        if (!lineRenderer) return false;
        return !lineRenderer.useWorldSpace;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!lineRenderer || !startPoint || !endPoint) return;

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPoint.position);
        lineRenderer.SetPosition(1, endPoint.position);
    }
}