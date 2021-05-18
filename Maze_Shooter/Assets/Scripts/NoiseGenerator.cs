using UnityEngine;
using Sirenix.OdinInspector;

[ExecuteAlways]
public class NoiseGenerator : MonoBehaviour
{
	[ToggleLeft]
	public bool realTime;
	[ToggleLeft, Tooltip("Noise output will be affected by the position of this object")]
	public bool usePosition;
	[ShowIf("usePosition"), Tooltip("frequency of the perlin noise projected on to world positions")]
	public float positionFrequency = 1;
	[Space]
    public float noiseSpeed;

	[ReadOnly]
	public Vector3 noise;

	[ReadOnly, Range(0, 1)]
	public float normalizedOutput;

    // The imaginary coordinates that sample on the perlin plane to generate noise
    // this comment is really smart sounding hah
    Vector2 _noiseSamplePos;

    // randomly assigned at start - determines the direction of movement of the sample point along the perlin plane
    Vector2 _noiseSampleDirection;

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.cyan;
		DrawNoiseAxis(Vector3.right * noise.x);
		DrawNoiseAxis(Vector3.up * noise.y);
		DrawNoiseAxis(Vector3.forward * noise.z);
	}

	void DrawNoiseAxis(Vector3 noiseVector)
	{
		Vector3 pos = transform.position + noiseVector * 5;
		Gizmos.DrawLine(transform.position, pos);
		Gizmos.DrawWireSphere(pos, .5f);
	}
    
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

		Vector3 totalPos = _noiseSamplePos;
		if (usePosition)
			totalPos += transform.position * positionFrequency;

        float x = Mathf.PerlinNoise(totalPos.x, -totalPos.z) - .5f;
        float y = Mathf.PerlinNoise(totalPos.y, -totalPos.z) - .5f;
		float z = Mathf.PerlinNoise(totalPos.x, -totalPos.y) - .5f;
        noise = new Vector3(x, y, z);

		normalizedOutput = x + .5f;
    }

	[Button]
    void RandomizeNoise()
    {
        _noiseSampleDirection = Random.onUnitSphere;
    }
}
