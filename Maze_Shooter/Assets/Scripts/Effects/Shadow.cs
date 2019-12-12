using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Unity.Mathematics;

public class Shadow : MonoBehaviour
{
    [AssetsOnly]
    public GameObject shadowPrefab;

    [Tooltip("Shadow is placed by raycasting down from this object. Which layers do you want to cast to?")]
    public LayerMask castingMask;
    GameObject shadowInstance;
    RaycastHit hit;
    
    // Start is called before the first frame update
    void Start()
    {
        shadowInstance = Instantiate(shadowPrefab, transform.position, quaternion.identity);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float y = shadowInstance.transform.position.y;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 100, castingMask))
        {
            y = hit.point.y;
        }
        
        shadowInstance.transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
}
