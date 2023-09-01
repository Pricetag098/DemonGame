using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class AnalyticLoader : MonoBehaviour
{
	public static AnalyticLoader instance { get; private set; }
	[SerializeField] string fileName = "Settings.cfg";
	[SerializeField] bool useEncryption = false;
	PositionData playerSettings;
	FileDataHandler<PositionData> fileHandler;
	List<IDataPersistance<PositionData>> persistanceObjects;

	private void Awake()
	{


		if (instance != null)
		{
			Debug.LogError("cannot have two eventsystems at once");
		}
		instance = this;
		persistanceObjects = FindAllDataPersistance();

		fileHandler = new FileDataHandler<PositionData>(Application.persistentDataPath, fileName, useEncryption);
		LoadGame();
	}

	public void NewGame()
	{
		playerSettings = new PositionData();
	}
	[ContextMenu("Open Thing")]
	void OpenThing()
	{
		Application.OpenURL(Application.persistentDataPath);
		
	}
	[ContextMenu("Test")]
	public void OpenFile()
	{
		System.Diagnostics.Process.Start("explorer.exe", "-p");
	}

	public void SaveGame()
	{
		foreach (IDataPersistance<PositionData> persistanceObject in persistanceObjects)
		{
			persistanceObject.SaveData(ref playerSettings);
		}

		fileHandler.Save(playerSettings);
	}

	public void LoadGame()
	{
		playerSettings = fileHandler.Load();

		if (playerSettings == null)
		{
			NewGame();
		}

		foreach (IDataPersistance<PositionData> persistanceObject in persistanceObjects)
		{
			persistanceObject.LoadData(playerSettings);
		}

	}

	private void OnApplicationQuit()
	{

	}

	List<IDataPersistance<PositionData>> FindAllDataPersistance()
	{
		IEnumerable<IDataPersistance<PositionData>> list = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance<PositionData>>();
		return new List<IDataPersistance<PositionData>>(list);
	}
}
