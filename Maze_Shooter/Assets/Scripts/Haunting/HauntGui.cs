using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HauntGui : MonoBehaviour
{
    public float destroyDelay = 1;
    public void End()
    {
        Destroy(gameObject, destroyDelay);
    }
}
