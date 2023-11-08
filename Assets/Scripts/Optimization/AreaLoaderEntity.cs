using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using Unity.Scenes;

public class AreaLoaderEntity : ComponentSystem
{
    private SceneSystem sceneSystem;

    protected override void OnCreate()
    {
        sceneSystem = World.GetOrCreateSystem<SceneSystem>();

        SubSceneLoader.AreaUpdate += OnAreaUpdate;
    }

    public void OnAreaUpdate()
    {
        switch (SubSceneLoader.CurrentArea)
        {
            case Areas.MainEntrance:
                // Areas To Load
                LoadSubScene(SubSceneReferences.Instance.Garden);
                LoadSubScene(SubSceneReferences.Instance.Courtyard);

                // Areas To Unload
                UnloadSubScene(SubSceneReferences.Instance.Graveyard);
                break;
            case Areas.Garden:
                // Areas To Load

                // Areas To Unload
                UnloadSubScene(SubSceneReferences.Instance.Graveyard);
                UnloadSubScene(SubSceneReferences.Instance.Courtyard);
                break;
            case Areas.Courtyard:
                // Areas To Load
                LoadSubScene(SubSceneReferences.Instance.Graveyard);

                // Areas To Unload
                UnloadSubScene(SubSceneReferences.Instance.Garden);
                break;
            case Areas.Graveyard:
                // Areas To Load

                // Areas To Unload
                UnloadSubScene(SubSceneReferences.Instance.Garden);
                break;
        }
    }

    protected override void OnUpdate()
    {
        //if(SubSceneLoader.UpdateSubScenes()) // MAKE SURE THIS SYNCS UP WITH AREALOADER
        //{
        //    switch(SubSceneLoader.CurrentArea)
        //    {
        //        case Areas.MainEntrance:
        //            // Areas To Load
        //            LoadSubScene(SubSceneReferences.Instance.Garden);
        //            LoadSubScene(SubSceneReferences.Instance.Courtyard);

        //            // Areas To Unload
        //            UnloadSubScene(SubSceneReferences.Instance.Graveyard);
        //            break;
        //        case Areas.Garden:
        //            // Areas To Load

        //            // Areas To Unload
        //            UnloadSubScene(SubSceneReferences.Instance.Graveyard);
        //            UnloadSubScene(SubSceneReferences.Instance.Courtyard);
        //            break;
        //        case Areas.Courtyard:
        //            // Areas To Load
        //            LoadSubScene(SubSceneReferences.Instance.Graveyard);

        //            // Areas To Unload
        //            UnloadSubScene(SubSceneReferences.Instance.Garden);
        //            break;
        //        case Areas.Graveyard:
        //            // Areas To Load

        //            // Areas To Unload
        //            UnloadSubScene(SubSceneReferences.Instance.Garden);
        //            break;
        //    }
        //}
    }

    private void LoadSubScene(SubScene subScene)
    {
        sceneSystem.LoadSceneAsync(subScene.SceneGUID);
    }

    private void UnloadSubScene(SubScene subScene)
    {
        sceneSystem.UnloadScene(subScene.SceneGUID);
    }
}
