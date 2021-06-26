using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EasySaveTester : MonoBehaviour
{
	public string fileName = "name me";
	public int myInt = 123;
	public List<string> funStrings = new List<string>();

	string filePath => "tests/" + fileName + ".es3";

	[ButtonGroup]
	void SaveToCache()
	{
		var settings = GetSettings();
		ES3.Save<int>("myInt", myInt,  settings);
		ES3.Save<List<string>>("test strings", funStrings, settings);
	}
	
	[ButtonGroup]
	void LoadFromCache()
	{
		var settings = GetSettings();
		myInt = ES3.Load<int>("myInt", settings);
		funStrings = ES3.Load<List<string>>("test strings", settings);
	}

	[ButtonGroup("file")]
	void SaveCacheToFile()
	{
		ES3.StoreCachedFile(filePath);
	}

	[ButtonGroup("file")]
	void LoadFromFile()
	{
		ES3.CacheFile(filePath);
		LoadFromCache();
	}

	ES3Settings GetSettings() 
	{
		ES3Settings settings = new ES3Settings(true);
		settings.location = ES3.Location.Cache;
		settings.path = filePath;
		return settings;
	}
}
 