using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
	public static SettingsManager instance { get; private set; }
	[SerializeField] string fileName = "Settings.cfg";
	[SerializeField] bool useEncryption = false;
	public PlayerSettings playerSettings;
	FileDataHandler<PlayerSettings> fileHandler;
	List<IDataPersistance<PlayerSettings>> persistanceObjects;

	private void Awake()
	{


		if (instance != null)
		{
			Debug.LogError("cannot have two eventsystems at once");
		}
		instance = this;
		persistanceObjects = FindAllDataPersistance();

		fileHandler = new FileDataHandler<PlayerSettings>(Application.persistentDataPath, fileName, useEncryption);
		LoadGame();
	}
    private void Start()
    {
        //LoadGame();
    }

    [ContextMenu("Open Thing")]
	void OpenThing()
	{
		Application.OpenURL(Application.persistentDataPath);
	}

	public void NewGame()
	{
		playerSettings = new PlayerSettings();
		
	}

	public void SaveGame()
	{
		foreach (IDataPersistance<PlayerSettings> persistanceObject in persistanceObjects)
		{
			persistanceObject.SaveData(ref playerSettings);
		}

		fileHandler.Save(playerSettings);
	}

	public void ClosedMenu()
	{
		playerSettings.hasOpened = true;
    }

    public void LoadGame()
	{
		playerSettings = fileHandler.Load();

		if (playerSettings == null)
		{
			NewGame();
		}

		foreach (IDataPersistance<PlayerSettings> persistanceObject in persistanceObjects)
		{
			//Debug.Log(persistanceObject);
			persistanceObject.LoadData(playerSettings);
		}

	}
    private void OnDestroy()
    {
        SaveGame();
    }

    

	List<IDataPersistance<PlayerSettings>> FindAllDataPersistance()
	{
		IEnumerable<IDataPersistance<PlayerSettings>> list = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistance<PlayerSettings>>();
		return new List<IDataPersistance<PlayerSettings>>(list);
	}
}
