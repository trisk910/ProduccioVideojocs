using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float sensX;
    public float sensY;

    private float xRotation;
    private float yRotation;

    public Transform orientation;

    void Start()
    {
        Cursor.lockState= CursorLockMode.Locked;
        Cursor.visible= false;
    }

    // Update is called once per frame
    void Update()
    {
        CameraRotation();
        
    }

    void CameraRotation()
    {
        float mouseX =  Input.GetAxis("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensY;

        
        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90);

        transform.rotation = Quaternion.Euler(xRotation,yRotation,0);
        orientation.rotation = Quaternion.Euler(0,yRotation,0);
    }
}
