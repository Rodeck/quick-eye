using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [Range(0.0f, 100.0f)]
    public float rotationSpeed = 10.0f;

    public Transform Target = null;

    public CameraMode mode = CameraMode.Smooth;

    public float rotSpeed = 250;
    public float damping = 10;

    private bool _isRotating = false;
    private Quaternion _desiredRotation = Quaternion.identity;
    private float previousFrameTime = 0;

    public float RotationTime = 1.0f;

    [Range(-20.0f, 20.0f)]
    public float Distance = -18.0f;

    [Range(-20.0f, 20.0f)]
    public float Heigth = 11.0f;

    [Range(-180.0f, 180.0f)]
    public float RotationY = 180.0f;

    [Range(-180.0f, 180.0f)]
    public float RotationX = 16.0f;

	// Use this for initialization
	void Start () {
        if (Target == null)
        {
            Debug.Log("Assign target!");
            return;
        }

        // Basic setup
        this.transform.position = CalculateStartingPosition();
        this.transform.Rotate(Vector3.up, RotationY);
        this.transform.Rotate(Vector3.right, RotationX);
	}

    private Vector3 CalculateStartingPosition()
    {
        return Target.position + new Vector3(0.0f, Heigth, Distance);
    }
	
	// Update is called once per frame
	void Update () {
        switch(mode)
        {
            case CameraMode.Smooth:
                {
                    SmoothMode();
                    break;
                }
            case CameraMode.Pulse:
                {
                    PulseMode();
                    break;
                }
        }
	}

    private void SmoothMode()
    {

        float currentTime = Time.realtimeSinceStartup;
        float deltaTime = currentTime - previousFrameTime;
        previousFrameTime = currentTime;

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            Debug.Log("Right");
            transform.RotateAround(Vector3.zero, Vector3.up * -1.0f, 20 * deltaTime * rotationSpeed);
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            Debug.Log("Left");
            transform.RotateAround(Vector3.zero, Vector3.up, 20 * deltaTime * rotationSpeed);
        }
    }

    private void PulseMode()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("Right");
            _isRotating = true;
            transform.RotateAround(Vector3.zero, Vector3.up, -90.0f);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) && !_isRotating)
        {
            Debug.Log("Left");
            transform.RotateAround(Vector3.zero, Vector3.up, 90.0f);
            _isRotating = true;
        }

        
    }
}

public enum CameraMode
{
    Smooth,
    Pulse
}
