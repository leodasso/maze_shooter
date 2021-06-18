using UnityEngine;

public interface ISaveable 
{
	public void Load();
	public void Save();

	public string Info();
}