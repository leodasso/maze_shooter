using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;

public class SaveFileSpawner : MonoBehaviour
{
	[AssetsOnly]
	public GameObject saveBubblePrefab;
	public Vector2 spawnArea;

	void OnDrawGizmos()
	{
		Gizmos.DrawCube(transform.position, (Vector3)spawnArea);
	}

	// Use this for initialization
	void Start () 
	{
		SpawnFiles();
	}

	void SpawnFiles()
	{
		foreach (var filename in ES3.GetFiles(GameMaster.saveFilesDirectory))
			SpawnFile(filename);
	}

	void SpawnFile(string filename)
	{
		int index = filename.LastIndexOf(".es3", StringComparison.Ordinal);
		string avatarName = filename.Substring(0, index);
		Vector3 random = new Vector3(Random.Range(-spawnArea.x/2, spawnArea.x/2), Random.Range(-spawnArea.y/2, spawnArea.y/2), 0);
		GameObject newFileBubble = Instantiate(saveBubblePrefab, transform.position + random, transform.rotation);
		SaveFileBubble bubble = newFileBubble.GetComponent<SaveFileBubble>();
		bubble.avatar = Resources.Load<SaveDataAvatar>("avatars/" + avatarName);
		bubble.Recalculate();
	}
}