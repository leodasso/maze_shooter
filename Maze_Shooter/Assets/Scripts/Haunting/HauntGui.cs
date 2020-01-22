using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HauntGui : MonoBehaviour
{
    public GameObject hauntPacketPrefab;
    [Tooltip("How many haunt packets this can contain.")]
    public int qty;
    int _hauntPackets;
    public float destroyDelay = 1;

    bool IsEmpty()
    {
        return _hauntPackets <= 0;
    }

    void Start()
    {
        _hauntPackets = qty;
    }

    public void End()
    {
        Destroy(gameObject, destroyDelay);
    }
}
