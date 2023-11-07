using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHealth : MonoBehaviour
{
    public int health;

    void Update()
    {
        if (health <= 0)
            Die();
    }

    public void Die()
    {
        print(this.gameObject.name + "destroyed");  // if you want a message display of destroyed object
        Destroy(this.gameObject);
    }

}

