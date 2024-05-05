using Scripts.InputManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using DG.Tweening;

public class PlayerCam : MonoBehaviour
{
    public float sensitivity = 4;

    public Transform orientation;
    public Transform camHolder;

    float xRotation;
    float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        // get mouse input
        Vector2 currentMouseVector = InputManager.Controls.Game.PointDelta.ReadValue<Vector2>();
        Vector2 resultMouseVector = sensitivity * Time.deltaTime * currentMouseVector;

        yRotation += resultMouseVector.x;

        xRotation -= resultMouseVector.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // rotate cam and orientation
        camHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void DoFov(float endValue)
    {
        //GetComponent<Camera>().DOFieldOfView(endValue, 0.25f);
    }

    public void DoTilt(float zTilt)
    {
        //transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.25f);
    }
}