using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    Transform playerBody;
    float pitch = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerBody = transform.parent.transform;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float lookX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float lookY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // yaw - horizontal - player
        playerBody.Rotate(Vector3.up * lookX);

        // pitch - vertical - player
        pitch -= lookY;

        pitch = Mathf.Clamp(pitch, -90f, 90f);

        transform.localRotation = Quaternion.Euler(pitch, 0, 0);
    }
}
