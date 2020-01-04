using UnityEngine;
using Sirenix.OdinInspector;

public class Shadow : MonoBehaviour
{
    [AssetsOnly]
    public GameObject shadowPrefab;

    [Tooltip("Shadow is placed by raycasting down from this object. Which layers do you want to cast to?")]
    public LayerMask castingMask;
    GameObject shadowInstance;
    RaycastHit hit;

    void OnEnable()
    {
        shadowInstance = Instantiate(shadowPrefab, transform.position, shadowPrefab.transform.rotation);
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

    void OnDisable()
    {
        if (shadowInstance)
            Destroy(shadowInstance);
    }
}
