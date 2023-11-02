using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectArea : MonoBehaviour
{
    public static Areas CurrentArea;
    public LayerMask areaLayer;
    [SerializeField] Transform orientation;
    [SerializeField] float TimerBetweenUpdates;

    private SpawnerManager manager;
    private Timer timer;

    private void Awake()
    {
        manager = FindObjectOfType<SpawnerManager>();
    }

    private void Start()
    {
        timer = new Timer(TimerBetweenUpdates);
        manager.UpdateSpawners(Areas.Courtyard, Areas.Null);
        GetArea();
    }

    private void Update()
    {
        if (timer.TimeGreaterThan)
        {
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
                    manager.UpdateSpawners(area.AreaId, CurrentArea);
                    CurrentArea = area.AreaId;
                }
            }
        }
    }
}
