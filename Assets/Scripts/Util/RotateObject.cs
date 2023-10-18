using UnityEngine;



public class RotateObject : MonoBehaviour
{
    public Vector3 rotationAxis = Vector3.up; // Axis around which to rotate
    public float rotationSpeed = 30f; // Speed of rotation in degrees per second



    private void Update()
    {
        // Calculate the rotation amount based on time and speed
        float rotationAmount = rotationSpeed * Time.deltaTime;



        // Rotate the object around the specified axis
        transform.Rotate(rotationAxis, rotationAmount);
    }
}