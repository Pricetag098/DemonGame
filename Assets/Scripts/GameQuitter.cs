using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameQuitter : MonoBehaviour
{
    // Start is called before the first frame update


    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
