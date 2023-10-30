//using Unity.Entities;
//using UnityEngine;
//using Unity.Mathematics;
//using Unity.Scenes;

//public class ChunkLoader : ComponentSystem
//{
//    private SceneSystem sceneSystem;

//    protected override void OnCreate()
//    {
//        sceneSystem = World.GetOrCreateSystem<SceneSystem>();
//    }

//    protected override void OnUpdate()
//    {
//        // add logic here
//        if(Input.GetKeyDown(KeyCode.Space))
//        {
//            LoadSubScene(SubSceneReferences.Instance.MainGate);
//            Debug.Log("Loaded");
//        }

//        if (Input.GetKeyDown(KeyCode.M))
//        {
//            UnloadSubScene(SubSceneReferences.Instance.MainGate);
//            Debug.Log("Unloaded");
//        }
//    }

//    private void LoadSubScene(SubScene subScene)
//    {
//        sceneSystem.LoadSceneAsync(subScene.SceneGUID);
//    }

//    private void UnloadSubScene(SubScene subScene)
//    {
//        sceneSystem.UnloadScene(subScene.SceneGUID);
//    }
//}
