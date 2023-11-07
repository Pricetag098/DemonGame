using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaLoader : MonoBehaviour
{
    public static Dictionary<Areas, Dictionary<AreaPriority, List<GameObject>>> Objects;

    private void Awake()
    {
        CreateDictionary();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (SubSceneLoader.UpdateSubScenes())
        {
            switch (SubSceneLoader.CurrentArea)
            {
                case Areas.MainEntrance:
                    // Areas To Load
                    

                    // Areas To Unload
                    
                    break;
                case Areas.Garden:
                    // Areas To Load

                    // Areas To Unload
                    
                    break;
                case Areas.Courtyard:
                    // Areas To Load
                   

                    // Areas To Unload
                    
                    break;
                case Areas.Graveyard:
                    // Areas To Load

                    // Areas To Unload
                    
                    break;
            }
        }
    }

    private void InitaliseDictionary()
    {
        AddArea(Areas.Courtyard);
        AddArea(Areas.MainEntrance);
        AddArea(Areas.Garden);
        AddArea(Areas.Graveyard);
    }

    private void AddArea(Areas area)
    {
        Objects.Add(area, new Dictionary<AreaPriority, List<GameObject>>());
        Objects[area].Add(AreaPriority.LOW_PRIORITY, new List<GameObject>());
        Objects[area].Add(AreaPriority.HIGH_PRIORITY, new List<GameObject>());
    }

#if UNITY_EDITOR
    public void CreateDictionary()
    {
        Objects = new Dictionary<Areas, Dictionary<AreaPriority, List<GameObject>>>();

        InitaliseDictionary();

        AreaObject[] objs = FindObjectsOfType<AreaObject>();

        foreach (AreaObject obj in objs) 
        {
            Objects[obj.ObjectArea][obj.ObjectPriority].Add(obj.transform.gameObject);
        }

        Debug.Log(Objects[Areas.Courtyard][AreaPriority.HIGH_PRIORITY].Count);
    }
#endif
}
