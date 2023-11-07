using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMovement : MonoBehaviour
{

    public float m_Speed = 12f;
    public float m_TurnSpeed = 180f;

    //or

    private Rigidbody m_Rigidbody;
    private float m_MovementInputValue; // takes the keyboard input to drive input values
    private float m_TurnInputValue;

    // Start is called before the first frame update
    private void Awake() // sets initial state/values 
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable() //allows us to turn the boat on by making it not kinematic, uses physics
    {
        m_Rigidbody.isKinematic = false;

        m_MovementInputValue = 0f; //resets movement to 0 at start?
        m_TurnInputValue = 0f;
    }

    private void OnDisable()
    {
        m_Rigidbody.isKinematic = true; //boat stops moving when turned off because its kinematic, no physics
    } 

    private void Update() 
    {
        m_MovementInputValue = Input.GetAxis("Vertical");
        m_TurnInputValue = Input.GetAxis("Horizontal");
    }  

    private void FixedUpdate() //every frame to adjust boat move and turn speeds.do physics calcs in fixedupdate.
    {
        Move();
        Turn();
    }

    private void Move()
    {
       Vector3 movement = -transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;
        //creates vector in direction of boat with magnitude calc per frame
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement); 

        //or
       // if (Input.GetKey(KeyCode.A))
            //GetComponent<Rigidbody>().AddForce(new Vector3(boatSpeed, 0, 0));

    }

    private void Turn()
    {
        float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime; //turn rate
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f); //makes it rotation in y
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
    }
}
