using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ViewSwing : MonoBehaviour
{
    [SerializeField] InputActionProperty lookInput;
	[SerializeField] InputActionProperty moveInput;
	[SerializeField] float resetStrength;
	[SerializeField] Vector3 lookEffectStrength = Vector3.one;
	[SerializeField] Vector3 moveEffectStrength = Vector3.one;
	[SerializeField] Vector3 limits = Vector3.one * 45;

	private void OnEnable()
	{
		lookInput.action.Enable();
		moveInput.action.Enable();
	}
	private void OnDisable()
	{
		lookInput.action.Disable();
		moveInput.action.Enable();
	}

	Vector3 rotation = Vector3.zero;
    // Update is called once per frame
    void FixedUpdate()
    {
		Vector2 inputDir = lookInput.action.ReadValue<Vector2>();
		Vector2 moveDir = moveInput.action.ReadValue<Vector2>();
		Vector3 rot = new Vector3
			(
			inputDir.y * lookEffectStrength.x + moveDir.y * moveEffectStrength.x,
			inputDir.x * lookEffectStrength.y + moveDir.x * moveEffectStrength.y,
			inputDir.x * lookEffectStrength.z + moveDir.x * moveEffectStrength.z
			);
		rotation = new Vector3
			(
				Mathf.Clamp(rotation.x + rot.x, -limits.x, limits.x),
				Mathf.Clamp(rotation.y + rot.y, -limits.y, limits.y),
				Mathf.Clamp(rotation.z + rot.z, -limits.z, limits.z)
			);
		rotation = Vector3.Slerp(rotation,Vector3.zero,resetStrength * Time.deltaTime);

		transform.localEulerAngles = rotation;

	}
	

}
