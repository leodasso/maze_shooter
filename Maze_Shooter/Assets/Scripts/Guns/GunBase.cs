using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arachnid;
using Sirenix.OdinInspector;

public class GunBase : MonoBehaviour
{    
    [ToggleLeft]
    public bool firing;
    public FloatReference startFiringDelay;
    public List<GunData> overrideGuns = new List<GunData>();
    public GunData gunData;

    public GunData GunData
    {
        get
        {
            if (overrideGuns.Count > 0) return overrideGuns[0];
            return gunData;
        }
    }
    
    [HideIf("HasGunData"), BoxGroup("local gun")]
    [MinMaxSlider(.1f, 60, true), Tooltip("Number of shots per second. The minimum is when the player is barely touching joystick," +
                                          " and max is when they're at full tilt.")]
    public Vector2 firingRate;
    [AssetsOnly, PreviewField, AssetList(AutoPopulate = false, Path = "Prefabs/Ammo"), BoxGroup("local gun"), HideIf("HasGunData")]
    public GameObject ammo;
    [Tooltip("This list is referenced by any ammo fired by this gun. The ammo will know " +
             "not to interact with these colliders. Prevents ammo from hitting the thing that " +
             "fired it on the very first frame.")]
    public List<Collider> collidersToIgnore = new List<Collider>();
    
    protected int level;
    float _startFiringTimer;
    protected bool HasGunData => GunData != null;
    protected GameObject Ammo => HasGunData ? GunData.ammo : ammo;
    protected bool AllowFiring => _startFiringTimer <= 0;
    
    // Use this for initialization
    protected virtual void Start()
    {
        _startFiringTimer = startFiringDelay.Value;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // Starting delay
        if (_startFiringTimer > 0)
        {
            _startFiringTimer -= Time.deltaTime;
        }
    }
    
    protected void CreateBullet(Vector2 offset, float angle)
    {
		offset = Math.Project2Dto3D(offset);
        Vector3 localOffset = transform.TransformPoint(offset);
        Debug.DrawLine(transform.position, localOffset, Color.yellow, 1);
        var newAmmo = Instantiate(Ammo, localOffset, transform.rotation);
        
		Projectile p = newAmmo.GetComponent<Projectile>();
		if (p)
			p.Fire(transform.forward);

        Hazard hazard = newAmmo.GetComponent<Hazard>();
        if (hazard)
        {
            hazard.ignoredColliders = new List<Collider>(collidersToIgnore);
            hazard.enabled = true;
        }
    }

	[Button]
	public virtual void Reload() {}
    
    public void AddOverride(GunData newData)
    {
        overrideGuns.Insert(0, newData);
    }

    public void RemoveOverride(GunData newData)
    {
        overrideGuns.Remove(newData);
    }
}
