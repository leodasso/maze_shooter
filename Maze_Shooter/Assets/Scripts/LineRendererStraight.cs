using UnityEngine;
using Sirenix.OdinInspector;

[ExecuteAlways]
public class LineRendererStraight : MonoBehaviour
{
    [InfoBox("Line Renderer should be set to use World Space for this component to work correctly", 
        InfoMessageType.Warning, "showLineRendererWarning")]
    public LineRenderer lineRenderer;

	[Range(2, 30)]
	public int vertices = 2;
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
        lineRenderer.positionCount = vertices;

		float max = vertices - 1;
		float progress = 0;
		Vector3 pos = startPoint.position;

		for (int i = 0; i < vertices; i++) 
		{
			progress = (float)i/max;
			pos = Vector3.Lerp(startPoint.position, endPoint.position, progress);
			lineRenderer.SetPosition(i, pos);
		}
    }
}