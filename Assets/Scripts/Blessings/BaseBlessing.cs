using Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBlessing : MonoBehaviour
{
    PooledObject PooledObject;
    [SerializeField] Optional<VfxSpawnRequest> vfx;
    private void Awake()
    {
        PooledObject = transform.parent.GetComponent<PooledObject>();
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

    }

    private void Delete()
    {
        PooledObject.Despawn();
    }
}
