using Arachnid;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

[TypeInfoBox("Soul is nourished by the light. If a spirit strays too far from the light, " +
             "soul is drained. When soul is empty, the spirit will be dragged into hell.")]
public class Soul : MonoBehaviour
{
    [Range(0, 1)]
    public float lightness = 1;
    public FloatReference darkness;
    
    public List<SoulLight> lightSources = new List<SoulLight>();
    
    // Start is called before the first frame update
    void Start()
    {
        UpdateDarkness();
    }

    // Update is called once per frame
    void Update()
    {
        lightness = 0;
        foreach (SoulLight soulLight in lightSources)
        {
            lightness += soulLight.LightIntensity(transform.position);
            if (lightness > 1)
                lightness = 1;
        }

        UpdateDarkness();
    }

    void UpdateDarkness()
    {
        darkness.Value = 1 - lightness;
    }

    void OnTriggerEnter(Collider other)
    {
        SoulLight soulLight = other.GetComponent<SoulLight>();
        if (!soulLight) return;
        
        if (!lightSources.Contains(soulLight))
            lightSources.Add(soulLight);
    }

    void OnTriggerExit(Collider other)
    {
        SoulLight soulLight = other.GetComponent<SoulLight>();
        if (!soulLight) return;

        lightSources.Remove(soulLight);
    }
}
