using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordManager : MonoBehaviour
{
    [HideInInspector] public RecordItem currentRecord;

    SoundPlayer musicPlayer;

    public List<GameObject> discs;

    GameObject discToDisable;

    BoxCollider InteractableUnityEvent;

    private void Awake()
    {
        musicPlayer = GetComponent<SoundPlayer>();

        InteractableUnityEvent = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        InteractableUnityEvent.enabled = false;
    }

    public void Interact()
    {
        musicPlayer.PlayClip(currentRecord.song);

        discToDisable.SetActive(false);

        discs[currentRecord.discNum].SetActive(true);

        discToDisable = discs[currentRecord.discNum];

        Invoke("DisableDisc", currentRecord.song.length);
    }

    public void HasDisc()
    {
        InteractableUnityEvent.enabled = true;
    }

    void DisableDisc()
    {
        discToDisable.SetActive(false);
    }

    public void StopPlaying()
    {
        musicPlayer?.Stop();
        discToDisable?.SetActive(false);
    }
}
