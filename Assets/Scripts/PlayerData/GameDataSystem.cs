using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;

public class GameDataSystem : MonoBehaviour
{
    public static GameDataSystem instance { get; private set; }
	[SerializeField] string fileName = "Data.dat";
	[SerializeField] bool useEncryption = false;
	GameData gameData;
	FileDataHandler<GameData> fileHandler;
	List<IDataPersistance<GameData>> persistanceObjects;
	private void Awake()
	{


		if (instance != null)
		{
			Debug.LogError("cannot have two eventsystems at once");
		}
		instance = this;
		persistanceObjects = FindAllDataPersistance();

		fileHandler = new FileDataHandler<GameData>(Application.persistentDataPath,fileName,useEncryption);
		LoadGame();
	}

	public void NewGame()
	{
		gameData = new GameData();
	}
	[ContextMenu("Open Thing")]
	void OpenThing()
	{
		Application.OpenURL(Application.persistentDataPath);
	}
	public void SaveGame()
	{
		foreach (IDataPersistance<GameData> persistanceObject in persistanceObjects)
		{
			persistanceObject.SaveData(ref gameData);
		}

		fileHandler.Save(gameData);
	}

	public void LoadGame()
	{
		gameData = fileHandler.Load();

		if(gameData == null)
		{
			NewGame();
		}

		foreach(IDataPersistance<GameData> persistanceObject in persistanceObjects)
		{
			persistanceObject.LoadData(gameData);
		}
		
	}

	private void OnApplicationQuit()
	{
		SaveGame();
	}

	List<IDataPersistance<GameData>> FindAllDataPersistance()
	{
		IEnumerable<IDataPersistance<GameData>> list = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance<GameData>>();
		return new List<IDataPersistance<GameData>>(list);
	}

}