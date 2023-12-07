using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSoundEvents : MonoBehaviour
{
    [System.Serializable]
    public class Event
    {
        public string soundType;

        public SoundPlayer soundPlayer;
    }

    public Event[] animEvents;

    public void PlaySound(int numInList)
    {
        animEvents[numInList].soundPlayer.Play();
    }
}
