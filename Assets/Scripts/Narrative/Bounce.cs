using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class Bounce : MonoBehaviour
{
    [SerializeField] private float radius;
    private float xPos;
    private float yPos;
    private float zPos;

    private Vector3 targetPos;
    private float speed;

    private AudioSource audioSource;
    [SerializeField] private List<AudioClip> clips;

    private bool escaped;
    [SerializeField] private List<Transform> escPos;
    private GameObject parent;
    private int escInt;

    [SerializeField] private RitualSpawner ritualSpawner;
    public int initialDemonCount;
    private bool check;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        parent = transform.parent.gameObject;
        escaped = false;
        check= false;
        SetTargetPos();
    }

    // Update is called once per frame
    void Update()
    {
        if (!escaped)
        {
            if (ritualSpawner.RitualActive)
            {
                if(!check)
                {
                    initialDemonCount = ritualSpawner.demonsLeft;
                    check = true;
                }
                speed = (initialDemonCount - ritualSpawner.demonsLeft) / (float)initialDemonCount;
            }
            //transform.DOLocalMove(targetPos, speed).SetSpeedBased(true).SetEase(Ease.OutQuart);
            if (transform.localPosition == targetPos)
            {
                SetTargetPos();
            }
        }
        else if (escaped && escInt < escPos.Count)
        {
            //transform.DOMove(escPos[escInt].position, speed).SetSpeedBased(true).SetEase(Ease.OutQuart);
            if (transform.position == escPos[escInt].position)
            {
                escInt++;
            }
        }
        if (escInt >= escPos.Count)
        {
            Door();
        }

    }
    void SetTargetPos()
    {
        xPos = Random.Range(-radius, radius);
        yPos = Random.Range(-Mathf.Sqrt(Mathf.Pow(radius, 2) - Mathf.Pow(xPos, 2)), Mathf.Sqrt(Mathf.Pow(radius, 2) - Mathf.Pow(xPos, 2)));
        int i = Random.Range(0, 2);
        if (i == 0)
        {
            zPos = Mathf.Sqrt(Mathf.Pow(radius, 2) - (Mathf.Pow(xPos, 2) + Mathf.Pow(yPos, 2)));
        }
        else
        {
            zPos = -Mathf.Sqrt(Mathf.Pow(radius, 2) - (Mathf.Pow(xPos, 2) + Mathf.Pow(yPos, 2)));
        }
        audioSource.clip = clips[Random.Range(0, clips.Count)];
        audioSource.Play();
        targetPos = new Vector3(xPos, yPos, zPos);
    }
    public void Escape()
    {
        escaped = true;
        parent.GetComponent<MeshRenderer>().enabled = false;
    }
    public void Door()
    {
        Destroy(this.gameObject);
    }
}
