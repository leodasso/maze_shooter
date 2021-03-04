using UnityEngine;
using Sirenix.OdinInspector;

public class NoiseGenerator : MonoBehaviour
{
	[ToggleLeft]
	public bool realTime;
    public float noiseSpeed;

	[ReadOnly]
	public Vector3 noise;

    // The imaginary coordinates that sample on the perlin plane to generate noise
    // this comment is really smart sounding hah
    Vector2 _noiseSamplePos;

    // randomly assigned at start - determines the direction of movement of the sample point along the perlin plane
    Vector2 _noiseSampleDirection;
    
    // Start is called before the first frame update
    void Start()
    {
        RandomizeNoise();
    }

    // Update is called once per frame
    void Update()
    {
		float t = realTime ? Time.unscaledDeltaTime : Time.deltaTime;

        _noiseSamplePos += t * _noiseSampleDirection * noiseSpeed;
        float x = Mathf.PerlinNoise(_noiseSamplePos.x, -_noiseSamplePos.x) - .5f;
        float y = Mathf.PerlinNoise(_noiseSamplePos.y, -_noiseSamplePos.y) - .5f;
		float z = Mathf.PerlinNoise(_noiseSamplePos.x, -_noiseSamplePos.y) - .5f;
        noise = new Vector3(x, y, z);
    }

    void RandomizeNoise()
    {
        _noiseSampleDirection = Random.onUnitSphere;
    }
}
