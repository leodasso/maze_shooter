using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogAnchor : MonoBehaviour
{
    public Vector2 panelOffset;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawCube(transform.position + (Vector3)panelOffset, Vector2.one * .2f);
    }

    public void DisplayDialog(Dialog dialog)
    {
        dialog.Display(gameObject);
    }
}
