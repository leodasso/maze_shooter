using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arachnid;
using Sirenix.OdinInspector;

public enum GunType {Player, Enemy}

public class GunBase : MonoBehaviour
{    
    [ToggleLeft]
    public bool firing;
    public GunType gunType;
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
    [AssetsOnly, HideIf("HasGunData"), BoxGroup("local gun")]
    public FiringPattern firingPattern;
    [AssetsOnly, PreviewField, AssetList(AutoPopulate = false, Path = "Prefabs/Ammo"), BoxGroup("local gun"), HideIf("HasGunData")]
    public GameObject ammo;
    
    [MinValue(0)]
    public int Level
    {
        get { return level; }
        set { level = Mathf.Clamp(value, 0, HasGunData ? GunData.MaxLevel : 0); }
    }
    
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
        Vector2 localOffset = transform.TransformPoint(offset);
        Debug.DrawLine(transform.position, localOffset, Color.yellow, 1);
        var newAmmo = Instantiate(Ammo, localOffset, transform.rotation);
        newAmmo.transform.Rotate(0, 0, angle, Space.World);
		
        newAmmo.layer = LayerMask.NameToLayer(gunType == GunType.Enemy ? 
            "EnemyBullets" : "PlayerBullets");
    }
    
    public void AddOverride(GunData newData)
    {
        overrideGuns.Insert(0, newData);
    }

    public void RemoveOverride(GunData newData)
    {
        overrideGuns.Remove(newData);
    }
}
