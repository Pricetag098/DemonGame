using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SessionDataSystem : MonoBehaviour
{
    public static SessionDataSystem instance { get; private set; }
    [SerializeField] string fileName = "Data.dat";
    [SerializeField] bool useEncryption = false;
    SessionData gameData;
    FileDataHandler<SessionData> fileHandler;
    List<IDataPersistance<SessionData>> persistanceObjects;
    private void Awake()
    {


        if (instance != null)
        {
            Debug.LogError("cannot have two eventsystems at once");
        }
        instance = this;
        fileName = System.Guid.NewGuid().ToString() + ".dat";
        persistanceObjects = FindAllDataPersistance();

        fileHandler = new FileDataHandler<SessionData>(Application.persistentDataPath, fileName, useEncryption,false);
        LoadGame();
    }

    public void NewGame()
    {
        gameData = new SessionData();
    }
    [ContextMenu("Open Thing")]
    void OpenThing()
    {
        Application.OpenURL(Application.persistentDataPath);
    }
    public void SaveGame()
    {
        foreach (IDataPersistance<SessionData> persistanceObject in persistanceObjects)
        {
            persistanceObject.SaveData(ref gameData);
        }

        fileHandler.Save(gameData);
    }

    public void LoadGame()
    {
        gameData = fileHandler.Load();

        if (gameData == null)
        {
            NewGame();
        }

        foreach (IDataPersistance<SessionData> persistanceObject in persistanceObjects)
        {
            persistanceObject.LoadData(gameData);
        }

    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    List<IDataPersistance<SessionData>> FindAllDataPersistance()
    {
        IEnumerable<IDataPersistance<SessionData>> list = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance<SessionData>>();
        return new List<IDataPersistance<SessionData>>(list);
    }
}
