using System.Collections;
using System.Collections.Generic;
using Arachnid;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

[TypeInfoBox("Moves along a path with physics.")]
public class PathMovement : MovementBase
{    

	public enum Direction {
		Forward,
		Back,
	}

	public enum PathEndAction {
		PingPong,
		Clamp,
	}

	[Space, Title("Path Tools")]
	[Tooltip("Choose a path for this character to move along")]
	public CharacterPath path;

	[ShowIf("HasPath"), Tooltip("The index of the path point this character will start at.")]
	[OnValueChanged("ProcessStartingIndex"), PropertyRange(0, "MaxIndex")]
	public int startingIndex;

	[ReadOnly, ShowIf("HasPath")]
	public int pathIndex;

	[ShowIf("HasPath"), OnValueChanged("PreviewDestination")]
	public Direction startDirection;

	[ShowIf("HasPathOption")]
	public PathEndAction pathEndAction = PathEndAction.PingPong;

	[ShowIf("HasPath"), Range(0.01f, 3)]
	[Tooltip("How close I have to get to the destination before considering it 'arrived at'")]
	public float destinationRadius = .5f;

	public List<PathEvent> pathEvents = new List<PathEvent>();

	int directionOnPath = 1;

	Vector3 destination;

	bool HasPath => path != null;

	bool HasPathOption => HasPath && !path.looped;

	[OnValueChanged("UpdatePathEvents")]
	int MaxIndex => HasPath ? path.pathPoints.Count - 1 : 1;

	// Memory of last processed point so we don't just process a point every frame if it's clamped
	int lastProcessedPoint = -99;


	protected override void Start()
	{
		directionOnPath = DirectionToInt(startDirection);
		pathIndex = startingIndex;
		base.Start();

		ProcessPathPoint();
	}

	protected override void FixedUpdate()
    {
		if (!path) 
		return;

		direction = (destination - transform.position).normalized;
	
		base.FixedUpdate();
    }

	protected override void Update()
	{
		base.Update();

		// Check for arriving at destination
		float sqRadius = destinationRadius * destinationRadius;

		// Ignore height/y-axis when calculating distance from waypoint 
		Vector3 flatPosition = new Vector3(transform.position.x, destination.y, transform.position.z);
		if (Vector3.SqrMagnitude(destination - flatPosition) < sqRadius) 
			ProcessPathPoint();
	}

	void ProcessStartingIndex()
	{
		if (!HasPath) return;
		startingIndex = Mathf.Clamp(startingIndex, 0, path.pathPoints.Count - 1);
		pathIndex = startingIndex;
		transform.position = path.GetWorldPos(startingIndex);
		PreviewDestination();
		UpdatePathEvents();
	}

	[Button, ShowIf("HasPath")]
	void UpdatePathEvents() 
	{
		if (!HasPath) return;
		foreach (var pathEvent in pathEvents) 
			pathEvent.SetMax(MaxIndex);
	}

	/// <summary>
	/// Only used to show direction arrow in editor
	/// </summary>
	void PreviewDestination() 
	{
		if (Application.isPlaying) return;

		if (!path.looped) {
			if (startingIndex == 0 && startDirection == Direction.Back) 
				destination = Vector3.zero;

			if (startingIndex == path.pathPoints.Count - 1 && startDirection == Direction.Forward) 
				destination = Vector3.zero;
		}

		destination = path.GetWorldPos(startingIndex + DirectionToInt(startDirection));
	}

	/// <summary>
	/// The vector to the next point. mainly used by the editor
	/// </summary>
	public Vector3 VectorToNext() 
	{
		return destination - transform.position;
	}


	void ProcessPathPoint() 
	{
		if (pathIndex == lastProcessedPoint) return;
		lastProcessedPoint = pathIndex;

		// actions on this point
		foreach(var pathEvent in pathEvents) {
			if (pathEvent.index == pathIndex)
				pathEvent.indexEvent.Invoke();
		}

		// Check for edges of the path 
		if (!path.IsInRange(pathIndex + directionOnPath)) {
			if (pathEndAction == PathEndAction.PingPong)
				ReverseDirection();

			else if (pathEndAction == PathEndAction.Clamp)
				return;
		}

		pathIndex += directionOnPath;
		destination = path.GetWorldPos(pathIndex);
	}

	void ReverseDirection() 
	{
		directionOnPath = -directionOnPath;
	}

	int DirectionToInt(Direction dir) => dir == Direction.Forward ? 1 : -1;
    
}

[System.Serializable]
public class PathEvent
{
	[PropertyRange(0, "Max")]
	public int index = 0;

	public UnityEvent indexEvent;

	[SerializeField, HideInInspector]
	int maxIndex = 1;

	public override string ToString() 
	{
		string s = "Event: ";
		for (int i = 0; i < indexEvent.GetPersistentEventCount(); i++) {
			var target = indexEvent.GetPersistentTarget(i);
			string targetName = target != null ? indexEvent.GetPersistentTarget(i).name : "(none)";
			s += targetName + " " + indexEvent.GetPersistentMethodName(i);
		}

		return s;
	}

	public int Max => maxIndex;

	public void SetMax(int newMax) {
		maxIndex = newMax;
		index = Mathf.Clamp(index, 0, maxIndex);
	}
}