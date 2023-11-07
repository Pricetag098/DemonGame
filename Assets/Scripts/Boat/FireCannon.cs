using UnityEngine;
using System.Collections;

public class FireCannon : MonoBehaviour
{
    //Drag in the Bullet Emitter from the Component Inspector.
    public GameObject Cannonball_Emitter; //defining here also lets you add transform so cannonballs shot in local, not global transform

    //Drag in the Bullet Prefab from the Component Inspector.
    public GameObject Cannonball;

    //Enter the Speed of the Bullet from the Component Inspector.
    public float Cannonball_Forward_Force;

    public float Cannonball_Lifespan;

    //cooldown on cannon firing
    //public float Cannon_Fire_Rate = 0.5f;
    bool reloading = false;
    bool canShoot = true;
    public float reloadTime;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space") && !reloading)
        {

            //The Bullet instantiation happens here.
            GameObject Temporary_Bullet_Handler;
            Temporary_Bullet_Handler = Instantiate(Cannonball, Cannonball_Emitter.transform.position, Cannonball_Emitter.transform.rotation) as GameObject;

            //Temporary_Bullet_Handler.transform.Rotate(Vector3.right * 90); //can rotate bullet if it isnt spherical

            //Retrieve the Rigidbody component from the instantiated Bullet and control it.
            Rigidbody Temporary_RigidBody;
            Temporary_RigidBody = Temporary_Bullet_Handler.GetComponent<Rigidbody>();

            //Tell the bullet to be "pushed" forward by an amount set by Bullet_Forward_Force. 
            Temporary_RigidBody.AddForce(Cannonball_Emitter.transform.forward * Cannonball_Forward_Force);
          

            //Basic Clean Up, set the Bullets to self destruct after 10 Seconds, I am being VERY generous here, normally 3 seconds is plenty.
            //Destroy(Temporary_Bullet_Handler, 2.0f);
            Destroy(Temporary_Bullet_Handler, Cannonball_Lifespan);

            {
                StartCoroutine(Reload());
            }
        }
        IEnumerator Reload()
        {
            reloading = true;
            canShoot = false;
            yield return new WaitForSeconds(reloadTime); //This causes the code to wait here for 3 seconds before continuing.
            canShoot = true;
            reloading = false;
        }
    }
}