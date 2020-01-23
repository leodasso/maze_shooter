using System.Collections;
using System.Collections.Generic;
using Arachnid;
using Unity.Mathematics;
using UnityEngine;

public class HauntGui : MonoBehaviour
{
    public FloatReference packetLaunchSpeed;
    public GameObject hauntPacketPrefab;
    [Tooltip("How many haunt packets this can contain.")]
    public int qty;
    int _hauntPackets;
    public float destroyDelay = 1;
    
    void Start()
    {
        _hauntPackets = qty;
    }

    public bool IsEmpty()
    {
        return _hauntPackets <= 0;
    }

    public void SendHauntPacket(GameObject target)
    {
        if (IsEmpty()) return;

        _hauntPackets--;
        GameObject hauntPacketInstance = Instantiate(hauntPacketPrefab, transform.position, quaternion.identity);
        var missile = hauntPacketInstance.GetComponent<SmartMissile3D>();
        missile.GetComponent<Rigidbody>().velocity = UnityEngine.Random.onUnitSphere * packetLaunchSpeed.Value;
        missile.customTarget = target.transform;
    }

    public void End()
    {
        Destroy(gameObject, destroyDelay);
    }
}
