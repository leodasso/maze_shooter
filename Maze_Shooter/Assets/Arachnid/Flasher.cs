using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arachnid;
using  Sirenix.OdinInspector;

[TypeInfoBox("Flashes the renderers off an on really quick to give that old school" +
             " arcade transparency look.")]
public class Flasher : MonoBehaviour
{
    public List<Renderer> flashingRenderers = new List<Renderer>();
    public FloatReference flashTime;

    bool _flashing = false;
    bool _showing;
    float _flashTimer;
    
    [Button]
    void GetAllRenderers()
    {
        flashingRenderers.Clear();
        flashingRenderers.AddRange(GetComponentsInChildren<Renderer>());
    }

    // Update is called once per frame
    void Update ()
    {
        if (!_flashing) return;
        _flashTimer += Time.deltaTime;
        if (!(_flashTimer >= flashTime.Value)) return;
        ToggleRenderers();
        _flashTimer = 0;
    }

    public void SetFlashing(bool isFlashing)
    {
        if (_flashing == isFlashing) return;

        _flashing = isFlashing;
        if (!isFlashing) SetRenderers(true);
    }

    public bool GetFlashing()
    {
        return _flashing;
    }
    
    void ToggleRenderers()
    {
        _showing = !_showing;
        SetRenderers(_showing);
    }

    void SetRenderers(bool showing)
    {
        foreach (var r in flashingRenderers)
        {
            if (!r) continue;
            r.enabled = showing;
        }
    }
}