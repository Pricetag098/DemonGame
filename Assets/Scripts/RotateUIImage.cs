using UnityEngine;
using UnityEngine.UI;

public class RotateUIImage : MonoBehaviour
{
    public float rotationSpeed = 60.0f; // Adjust this value to change the rotation speed in degrees per second.
    public bool clockwise = true;       // Toggle to change the rotation direction.

    private RectTransform imageTransform;
    private int rotationDirection = 1;   // 1 for clockwise, -1 for counterclockwise

    private void Start()
    {
        // Get the RectTransform of the UI Image.
        imageTransform = GetComponent<RectTransform>();

        // Set the initial rotation direction based on the "clockwise" variable.
        rotationDirection = clockwise ? 1 : -1;
    }

    private void Update()
    {
        // Rotate the UI Image by the specified speed and direction.
        imageTransform.Rotate(Vector3.forward * rotationSpeed * rotationDirection * Time.deltaTime);

        // Check if the rotation angle exceeds 360 degrees or goes below 0 degrees, and reset it to keep looping.
        if (imageTransform.eulerAngles.z >= 360.0f || imageTransform.eulerAngles.z <= 0.0f)
        {
            imageTransform.eulerAngles = new Vector3(0.0f, 0.0f, clockwise ? 0.0f : 360.0f);
        }
    }
}
