using Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class BaseBlessing : MonoBehaviour
{
    PooledObject PooledObject;

    private void Awake()
    {
        PooledObject = transform.parent.GetComponent<PooledObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 6)
        {
            Activate(other.GetComponentInParent<PlayerInput>().gameObject);
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
