using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using Sirenix.OdinInspector;

public class Gate : MonoBehaviour
{
    [ValidateInput("ValidateStars", "Required stars count must match the sum of stars in gate lights. Either adjust the count or " +
                                    "add/remove gate lights.", ContinuousValidationCheck = true)]
    public int requiredStars = 5;
    public List<GateLight> gateLights = new List<GateLight>();
    [AssetsOnly, Tooltip("The prop constellation that gets pulled out of the ghost and put into the gate lights")]
    public GameObject propConstellationPrefab;
    public Collection player;

    [ShowInInspector, ReadOnly]
    GameObject _player;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetPlayer()
    {
        _player = player.GetElement(0).gameObject;
    }

    bool ValidateStars(int inputStars)
    {
        int sum = 0;
        foreach (GateLight gateLight in gateLights)
        {
            sum += gateLight.stars;
        }

        return sum == inputStars;
    }

    [Button]
    void GetGateLights()
    {
        gateLights.Clear();
        gateLights.AddRange(GetComponentsInChildren<GateLight>());
    }
}
