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
    //private GameObject parent;
    [SerializeField] private int escInt;

    [SerializeField] private RitualSpawner ritualSpawner;
    [SerializeField] private RitualDoor ritualDoor;
    public int initialDemonCount;
    private bool check;
    private bool finalCheck;
    [SerializeField] private int oldInt;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //parent = transform.parent.gameObject;
        escaped = false;
        check= false;
        speedCheck = false;
        finalCheck = false;
        targetPos = new Vector3(3, 4, 5);
    }

    // Update is called once per frame
    void Update()
    {
//        if (!escaped)
//        {
//            if (ritualSpawner.RitualActive)
//            {
//                if(!check)
//                {
//                    initialDemonCount = ritualSpawner.demonsLeft;
//                    check = true;
//                }
//                speed = maxSpeed / ((initialDemonCount - ritualSpawner.demonsLeft) / (float)initialDemonCount);
///*                if (speed > 1000f)
//                {
//                    speed = 999;
//                }*/
//                if (!speedCheck && speed < 999)
//                {
//                    SetTargetPos();
//                    speedCheck = true;
//                }
//            }
//            //transform.DOLocalMove(targetPos, speed).SetSpeedBased(true).SetEase(Ease.OutQuart);
//            if (transform.localPosition == targetPos)
//            {
//                SetTargetPos();
//            }
//        }
        if (escaped)
        {
            //transform.DOMove(escPos[escInt].position, speed).SetSpeedBased(true).SetEase(Ease.OutQuart);
            if (transform.position == escPos[oldInt].position)
            {
                if (oldInt >= escPos.Count - 1 && finalCheck == false)
                {
                    Door();
                    return;
                }
                else
                {
                    UpdateEscTween();
                    return;
                }
            }
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
        transform.DOMove(escPos[escInt].position, 5f).SetEase(Ease.Linear);
        if(escInt != 0)
        {
            oldInt++;
        }
        escInt++;
    }
    public void Escape()
    {
        escaped = true;
        UpdateEscTween();
        //parent.GetComponent<MeshRenderer>().enabled = false;
    }
    public void Door()
    {
        transform.parent = doorParent;
        ritualDoor.AddRitual();
        finalCheck = true;
        escaped = false;
        //Destroy(this.gameObject);
    }
}
