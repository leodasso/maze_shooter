using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Sirenix.OdinInspector;

[ExecuteAlways]
public class ParticlesAlongPath : MonoBehaviour
{
	public CinemachinePathBase path;
	public new ParticleSystem particleSystem;

	[MinValue(.01f)]
	public float spacing = .5f;

	int count = 0;

	ParticleSystem.Particle[] particles;

    // Start is called before the first frame update
    void Start()
    {
        
    }

	void Update() 
	{
		if (!path || !particleSystem) return;
		UpdateParticles();
	}


	void UpdateParticles() 
	{
		if (!path) return;

		// get the count of particles needed to fit on the line
		int newCount = Mathf.RoundToInt(path.PathLength / spacing);
		if (newCount != count) {
			count = newCount;
			ReInitialize();
		}

		if (particles == null) ReInitialize();

        int particleCount = particleSystem.GetParticles(particles);

        for (int i = 0; i < particleCount; i++)
        {
			Vector3 pos = path.EvaluatePositionAtUnit(i * spacing, CinemachinePathBase.PositionUnits.Distance);
			particles[i].position = pos;
			particles[i].startColor = Color.white;
        }
        particleSystem.SetParticles(particles, particles.Length);
	}

	void ReInitialize() {
		particles = new ParticleSystem.Particle[count];
	}
}
