using Arachnid;
using UnityEngine;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using Random = UnityEngine.Random;


public class HauntGlobEmitter : MonoBehaviour
{
    public FloatReference emitVelocity;
    public FloatReference quantity;
    [ToggleLeft]
    public bool emitOnDisable = true;
    [AssetsOnly]
    public GameObject hauntGlobPrefab;

    void OnDisable()
    {
        if (emitOnDisable)
            Emit(quantity.Value);
    }

    [Button]
    public void Emit(float qty)
    {
        #if UNITY_EDITOR
        if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode || UnityEditor.EditorApplication.isPaused)
        {
            Debug.LogWarning(name + " tried to instantiate when editor is paused or about to change playmode.", gameObject);
            return;
        }
        #endif
        
        float remaining = qty;
        while (remaining > 0)
        {
            // Determine a random value to assign to the glob
            float max = Mathf.Clamp(.1f, 0, remaining);
            float min = .02f;
            float emitValue = Random.Range(min, max);
            remaining -= emitValue;
            
            // Instantiate the glob and set the value
            HauntGlob newGlob = Instantiate(hauntGlobPrefab, transform.position, quaternion.identity)
                .GetComponent<HauntGlob>();
            newGlob.SetValue(emitValue);
            
            // give a random force to the new glob
            Rigidbody2D globRigidbody = newGlob.GetComponent<Rigidbody2D>();
            globRigidbody.velocity = Random.Range(0, emitVelocity.Value) * Random.onUnitSphere;
        }
    }
}
