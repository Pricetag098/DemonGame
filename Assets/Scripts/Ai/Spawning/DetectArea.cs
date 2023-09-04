using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectArea : MonoBehaviour
{
    public Areas CurrentArea;
    public LayerMask areaLayer;
    [SerializeField] Transform orientation;
    [SerializeField] float TimerBetweenUpdates;

    private SpawnerManager manager;
    private float timer;

    private void Awake()
    {
        manager = FindObjectOfType<SpawnerManager>();
        CurrentArea = Areas.Courtyard;
    }

    private void Start()
    {
        manager.UpdateSpawners(CurrentArea);
        GetArea();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (HelperFuntions.TimerGreaterThan(timer, TimerBetweenUpdates))
        {
            timer = 0;
            GetArea();
        }
    }

    void GetArea()
    {
        if(Physics.Raycast(orientation.position, Vector3.down, out RaycastHit hit, 20, areaLayer))
        {
            if (hit.collider.TryGetComponent<Area>(out Area area))
            {
                if(CurrentArea != area.AreaId)
                {
                    manager.UpdateSpawners(CurrentArea);
                    CurrentArea = area.AreaId;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(orientation.position, orientation.position + (Vector3.down * 5));
    }
}
