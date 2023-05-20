using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{
    public float sens;    

    private float xRotation;
    private float yRotation;

    public Transform orientation;

    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TextMeshProUGUI sensitivityText;

    void Start()
    {
        Cursor.lockState= CursorLockMode.Locked;
        Cursor.visible= false;
        sensitivitySlider.onValueChanged.AddListener(UpdateSensitivity);
        UpdateSensitivity(sensitivitySlider.value);  // Set initial sensitivity
    }

    // Update is called once per frame
    void Update()
    {
        CameraRotation();
        
    }

    void CameraRotation()
    {
        float mouseX =  Input.GetAxis("Mouse X") * Time.deltaTime * sens;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sens;

        
        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90);

        transform.rotation = Quaternion.Euler(xRotation,yRotation,0);
        orientation.rotation = Quaternion.Euler(0,yRotation,0);
    }
    private void UpdateSensitivity(float value)
    {
        sens = value;
        sensitivityText.text = value.ToString("F2");  // Update sensitivity text
    }
}
