using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.UI;


public class VRCameraController : MonoBehaviour
{
	public bool invertLookX, invertLookY;

	public float sensitivityX = 1f, sensitivityY = 1, verticalAngleClamp;
	private float rotY, rotX;

	private void Start()
	{
		Vector3 rot = transform.localRotation.eulerAngles;
		rotY = rot.y;
		rotX = rot.x;

		//enables gyrometer-based movement for portable devices.
		Input.gyro.enabled = true;
		Screen.orientation = ScreenOrientation.LandscapeLeft;
	}

	private void Update()
	{
		//will only enable mouse control while in the editor, and Gyro control if not.
		#if UNITY_EDITOR
		MouseControl();
		#else
		GyroControl();
		#endif
	}

	/// <summary>
	//When called in Update(), adjusts the camera based on mouse movement. Only applied when left mouse button is held.
	/// </summary>
	private void MouseControl()
	{
		if (Input.GetMouseButton(0))
		{
			//allows the user to invert mouse controls from the Unity inspector if desired
			float mouseX = Input.GetAxis("Mouse X") * (invertLookX ? -1 : 1) * 100;
			float mouseY = Input.GetAxis("Mouse Y") * (invertLookY ? 1 : -1) * 100;

			rotY += mouseX * sensitivityX * Time.deltaTime;
			rotX += mouseY * sensitivityY * Time.deltaTime;

			//prevents the user from rotating around completely on the Y axis and ending up "upside down".
			rotX = Mathf.Clamp(rotX, -verticalAngleClamp, verticalAngleClamp);

			//applies the rotation
			Quaternion rotation = Quaternion.Euler(rotX, rotY, 0.0f);
			transform.rotation = rotation;
		}
	}

	/// <summary>
	/// Simply translates gyrometer attitude to camera rotation
	/// </summary>
	private void GyroControl()
	{
		transform.localRotation = 
			// Neutral position is phone held upright, not flat on a table.
			Quaternion.Euler(90f, 0f, 0f) *

			// Sensor reading, assuming default `Input.compensateSensors == true`.
			Input.gyro.attitude *

			// So image is not upside down.
			Quaternion.Euler(0f, 0f, 180f);
	}
}
