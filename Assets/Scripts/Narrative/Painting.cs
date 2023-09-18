using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Painting : MonoBehaviour
{
    public GameObject completionObject;
    public GameObject painting;
    public GameObject salt;
    public GameObject interactable;
    public string text;

    public SoundPlayer failSound;
    public SoundPlayer successSound;


    public void CorrectPainting()
    {
        completionObject.SetActive(true);
        successSound.Play();
    }

    public void FailedPainting()
    {
        completionObject.SetActive(false);
    }

    public void FinishedPuzzle()
    {
        painting.SetActive(false);
        salt.SetActive(true);
    }

    public void SetDisabled()
    {
        completionObject.SetActive(false);
        interactable.SetActive(false);
    }
}
