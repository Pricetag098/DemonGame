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
