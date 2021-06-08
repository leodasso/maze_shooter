using UnityEngine;
using Sirenix.OdinInspector;

[AddComponentMenu("Shadows/Shadow")]
public class Shadow : MonoBehaviour
{
	
    [AssetsOnly, AssetList(Path = "Prefabs/Shadows/")]
	[PreviewField(120, ObjectFieldAlignment.Center)]
    public GameObject shadowPrefab;

    public float shadowScale = 1;

	[ToggleLeft, Tooltip("If true, will cast in the camera down direction rather than world down. Useful when combined with billboards")]
	public bool castCameraDown;

    [Tooltip("Shadow is placed by raycasting down from this object. Which layers do you want to cast to?")]
    public LayerMask castingMask;
    GameObject shadowInstance;
    RaycastHit hit;
    ShadowObject _shadowObject;
	Transform _camera;

	static GameObject shadowParent;

	Vector3 CastingDir() 
	{
		if (!castCameraDown) return Vector3.down;
		if (!_camera) return Vector3.down;
		return -_camera.up;
	}

	void OnDrawGizmosSelected()
	{
		const float shadowScaleConstant = 2;

		Gizmos.color = new Color(0, 0, 0, .1f);
		Gizmos.DrawSphere(transform.position, shadowScale * shadowScaleConstant);
		Gizmos.color = Color.black;
		GizmoExtensions.DrawCircle(transform.position, shadowScale * shadowScaleConstant);
	}

	void Start()
    {
		if (!Camera.main) return;
        _camera = Camera.main.transform;
    }

    void OnEnable()
    {
		if (shadowParent == null) 
			shadowParent = new GameObject("Shadows");

        shadowInstance = Instantiate(shadowPrefab, transform.position, shadowPrefab.transform.rotation);
		shadowInstance.transform.parent = shadowParent.transform;
        _shadowObject = shadowInstance.GetComponent<ShadowObject>();
		_shadowObject.SetScale(shadowScale);
    }
    
    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 shadowPos = transform.position;
        if (Physics.Raycast(transform.position, CastingDir(), out hit, 100, castingMask))
        {
            shadowPos = hit.point;
        }
        
        shadowInstance.transform.position = shadowPos;
        if (_shadowObject)
            _shadowObject.SetDistance(transform.position.y);
    }

    void OnDisable()
    {
        if (shadowInstance)
            Destroy(shadowInstance);
    }
}
