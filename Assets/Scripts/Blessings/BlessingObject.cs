using Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlessingObject : MonoBehaviour
{
    PooledObject PooledObject;
    [SerializeField] Optional<VfxSpawnRequest> vfx;
    BlessingStatusHandler statusHandler;
    [SerializeField] Blessing blessing;
    private void Awake()
    {
        PooledObject = transform.parent.GetComponent<PooledObject>();
        statusHandler = FindObjectOfType<BlessingStatusHandler>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 6)
        {
            GameObject target = other.GetComponentInParent<PlayerInput>().gameObject;

		 Activate(target);
            if(vfx.Enabled)
            vfx.Value.Play(transform.position, target.transform.position - transform.position);
            Delete();
        }
    }

    protected virtual void Activate(GameObject player)
    {
        Instantiate(blessing).Equip(statusHandler);
    }

    private void Delete()
    {
        PooledObject.Despawn();
    }
}
