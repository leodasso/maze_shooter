using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PlayMaker;
using Cinemachine;

[ExecuteAlways]
public class ArcMover : MonoBehaviour
{
    public GameObject haunted;

	[SerializeField]
	List<Transform> ghostChunks = new List<Transform>();

	[SerializeField]
	PlayMakerFSM playMaker;

	[SerializeField, Tooltip("Calls event 'doTransition'")]
	CinemachinePath path;

    // Update is called once per frame
    void Update()
    {
    }

	public void DoTransition(GameObject newHaunted, Vector3 newReturnPos, float duration = .5f)
	{
		SetTransitionDuration(duration);

		PlaceGhostChunks(newHaunted.transform.position);

		path.m_Waypoints[0].position = transform.InverseTransformPoint(newHaunted.transform.position);
		path.m_Waypoints[1].position = transform.InverseTransformPoint(newReturnPos);
		playMaker.SendEvent("doTransition");
	}

	void PlaceGhostChunks(Vector3 pos)
	{
		foreach (var chunk in ghostChunks) 
			chunk.position = pos;
	}

	void SetTransitionDuration(float duration) 
	{
		playMaker.FsmVariables.GetFsmFloat("transitionDuration").Value = duration;
	}
}