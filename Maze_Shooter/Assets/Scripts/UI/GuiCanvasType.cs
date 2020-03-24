using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Ghost/GUI Canvas Type")]
public class GuiCanvasType : ScriptableObject
{
    [SerializeField, ShowInInspector]
    GameObject canvasPrefab;
    
    [ReadOnly, SerializeField, ShowInInspector]
    GameObject canvasInstance;

    /// <summary>
    /// Returns the instance of this canvas. Creates one if none exist.
    /// </summary>
    public GameObject GetInstance()
    {
        if (canvasInstance) 
            return canvasInstance;
        canvasInstance = Instantiate(canvasPrefab);
        return canvasInstance;
    }
}
