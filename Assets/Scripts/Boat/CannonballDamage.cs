using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonballDamage : MonoBehaviour
{

    public int damage;
    private GameObject target;



    private void OnTriggerEnter(Collider other)
    {
        //if (other.tag == "Player")
        if (other.tag == "Player 2")
        {
            target = other.gameObject;
            target.GetComponent<ShipHealth>().health -= damage;
        }
    }
}