using Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class BaseBlessing : MonoBehaviour
{
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
        Destroy(transform.parent.gameObject);
    }
}
