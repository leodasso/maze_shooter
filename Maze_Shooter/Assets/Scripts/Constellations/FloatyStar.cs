using UnityEngine;
using Sirenix.OdinInspector;

public class FloatyStar : MonoBehaviour
{
    public float noiseIntensity;
    public float noiseSpeed;
    Vector3 _initLocalPos;
    
    // The total noise that gets added to init local position every frame
    Vector2 _noiseVector;
    
    // The imaginary coordinates that sample on the perlin plane to generate noise
    // this comment is really smart sounding hah
    Vector2 _noiseSamplePos;

    // randomly assigned at start - determines the direction of movement of the sample point along the perlin plane
    Vector2 _noiseSampleDirection;
    
    // Start is called before the first frame update
    void Start()
    {
        _initLocalPos = transform.localPosition;
        RandomizeNoise();
    }

    // Update is called once per frame
    void Update()
    {
        _noiseSamplePos += Time.deltaTime * _noiseSampleDirection * noiseSpeed;
        float x = Mathf.PerlinNoise(_noiseSamplePos.x, -_noiseSamplePos.x) - .5f;
        float y = Mathf.PerlinNoise(_noiseSamplePos.y, -_noiseSamplePos.y) - .5f;
        _noiseVector = new Vector2(x, y) * noiseIntensity;

        transform.localPosition = _initLocalPos + (Vector3)_noiseVector;
    }

    void OnEnable()
    {
        _initLocalPos = transform.localPosition;
    }

    [Button]
    void RandomizeNoise()
    {
        _noiseSampleDirection = Random.onUnitSphere;
    }
}
