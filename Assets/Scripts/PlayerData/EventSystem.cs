using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;

public class EventSystem : MonoBehaviour
{
    public static EventSystem instance { get; private set; }

	GameData gameData;

	List<IDataPersistance> persistanceObjects;
	private void Awake()
	{


		if (instance != null)
		{
			Debug.LogError("cannot have two eventsystems at once");
		}
		instance = this;
		persistanceObjects = FindAllDataPersistance();
		LoadGame();
	}

	public void NewGame()
	{
		gameData = new GameData();
	}

	public void SaveGame()
	{
		foreach (IDataPersistance persistanceObject in persistanceObjects)
		{
			persistanceObject.SaveData(ref gameData);
		}
	}

	public void LoadGame()
	{


		if(gameData == null)
		{
			NewGame();
		}

		foreach(IDataPersistance persistanceObject in persistanceObjects)
		{
			persistanceObject.LoadData(gameData);
		}
		
	}

	private void OnApplicationQuit()
	{
		SaveGame();
	}

	List<IDataPersistance> FindAllDataPersistance()
	{
		IEnumerable<IDataPersistance> list = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance>();
		return new List<IDataPersistance>(list);
	}

}