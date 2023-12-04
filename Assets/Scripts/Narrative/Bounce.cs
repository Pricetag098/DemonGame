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
    public float maxSpeed;
    private bool speedCheck;
    public Transform doorParent;

    private AudioSource audioSource;
    [SerializeField] private List<AudioClip> clips;

    private bool escaped;
    [SerializeField] private List<Transform> escPos;
    private GameObject parent;
    private int escInt;

    [SerializeField] private RitualSpawner ritualSpawner;
    [SerializeField] private RitualDoor ritualDoor;
    public int initialDemonCount;
    private bool check;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        parent = transform.parent.gameObject;
        escaped = false;
        check= false;
        speedCheck = false;
        targetPos = new Vector3(3, 4, 5);
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
                speed = maxSpeed / ((initialDemonCount - ritualSpawner.demonsLeft) / (float)initialDemonCount);
/*                if (speed > 1000f)
                {
                    speed = 999;
                }*/
                if (!speedCheck && speed < 999)
                {
                    SetTargetPos();
                    speedCheck = true;
                }
            }
            //transform.DOLocalMove(targetPos, speed).SetSpeedBased(true).SetEase(Ease.OutQuart);
            if (transform.localPosition == targetPos)
            {
                SetTargetPos();
            }
        }
        else if (escaped && escInt < escPos.Count - 1)
        {
            //transform.DOMove(escPos[escInt].position, speed).SetSpeedBased(true).SetEase(Ease.OutQuart);
            if (transform.position == escPos[escInt].position)
            {
                escInt++;
                UpdateEscTween();
            }
        }
        if (escInt >= escPos.Count - 1)
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
        transform.DOLocalMove(targetPos, speed).SetEase(Ease.Linear);
    }
    void UpdateEscTween()
    {
        transform.DOMove(escPos[escInt].position, 10f).SetEase(Ease.Linear);
    }
    public void Escape()
    {
        escaped = true;
        UpdateEscTween();
        parent.GetComponent<MeshRenderer>().enabled = false;
        transform.parent = doorParent;
    }
    public void Door()
    {
        ritualDoor.AddRitual();
        //Destroy(this.gameObject);
    }
}
