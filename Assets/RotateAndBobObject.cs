using UnityEngine;

 

public class RotateAndBobObject : MonoBehaviour

{

    public Vector3 rotationAxis = Vector3.up; // Axis around which to rotate

    public float rotationSpeed = 30f; // Speed of rotation in degrees per second

    public float bobSpeed = 1f; // Speed of bobbing motion

    public float bobHeight = 0.5f; // Height of the bobbing motion



    private Vector3 startPosition;



    private void Start()

    {

        startPosition = transform.localPosition;

    }



    private void Update()

    {

        // Calculate the rotation amount based on time and speed

        float rotationAmount = rotationSpeed * Time.deltaTime;



        // Rotate the object around the specified axis

        transform.Rotate(rotationAxis, rotationAmount);



        // Calculate the bobbing motion using the sine function

        float bobOffset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;



        // Update the object's position with bobbing motion

        transform.localPosition = startPosition + new Vector3(0f, bobOffset, 0f);

    }

}

