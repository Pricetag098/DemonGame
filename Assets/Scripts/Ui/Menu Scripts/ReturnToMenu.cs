using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToMenu : MonoBehaviour
{
    public float inputDelay;
    public float returnDelay;

    private float timer = 0;

    bool ended = false;

    public GameObject[] panels;

    private void Update()
    {
        if (ended)
        {
            timer += Time.deltaTime;
        }

        if (timer >= inputDelay)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Transition();
            }
        }

        if (timer >= returnDelay)
        {
            Transition();
        }
    }

    public void EndMatch()
    {
        ended = true;
    }

    public void Transition()
    {
        ended = false;
        timer = 0;
        panels[0].SetActive(false);
        panels[1].SetActive(true);
    }
}
