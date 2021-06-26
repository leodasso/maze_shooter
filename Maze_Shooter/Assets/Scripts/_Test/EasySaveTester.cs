using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EasySaveTester : MonoBehaviour
{
	public int myInt = 123;
	public List<string> funStrings = new List<string>();

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

	ES3Settings GetSettings() 
	{
		ES3Settings settings = new ES3Settings();
		settings.location = ES3.Location.Memory;
		settings.path = "cache.es3";
		return settings;
	}
}
 