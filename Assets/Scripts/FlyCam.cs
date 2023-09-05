using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class FlyCam : MonoBehaviour
{
    public float speed = 10;
    public float sensitivity = 10;
    public InputActionProperty action;

    bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        action.action.Enable();
        action.action.performed += Pause;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition += Time.deltaTime * Input.GetAxis("Vertical") * transform.forward * speed;
        transform.localPosition += Time.deltaTime * Input.GetAxis("Horizontal") * transform.right * speed;
        transform.localEulerAngles += new Vector3(-Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivity, Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivity, 0);
    }

	void Pause(InputAction.CallbackContext context)
	{
        paused = !paused;
		if (paused)
		{
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
		else
		{
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
	}
}