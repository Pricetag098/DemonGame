using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using Unity.Scenes;

public class ChunkLoader : ComponentSystem
{
    private SceneSystem sceneSystem;

    protected override void OnCreate()
    {
        sceneSystem = World.GetOrCreateSystem<SceneSystem>();
    }

    protected override void OnUpdate()
    {
        if(SubSceneLoader.UpdateSubScenes())
        {
            switch(SubSceneLoader.CurrentArea)
            {
                case Areas.MainEntrance:
                    // Areas To Load
                    LoadSubScene(SubSceneReferences.Instance.Garden);

                    // Areas To Unload
                    UnloadSubScene(SubSceneReferences.Instance.Graveyard);
                    break;
                case Areas.Garden:
                    // Areas To Load

                    // Areas To Unload
                    UnloadSubScene(SubSceneReferences.Instance.Graveyard);
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
