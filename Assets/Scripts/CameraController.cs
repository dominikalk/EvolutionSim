using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Rigidbody myRigid;
    SimSettings simSettings;

    [SerializeField] float speed = 100f;
    [SerializeField] float rotSpeed = 2;

    float yaw = 0f;
    float pitch = 0f;

    float initX;
    float initY;

    // Start is called before the first frame update
    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
        simSettings = FindObjectOfType<SimSettings>();
        initX = transform.eulerAngles.x;
        initY = transform.eulerAngles.y;
        pitch += initX;
        yaw += initY;
    }

    // Update is called once per frame
    void Update()
    {
        if(simSettings.lockedScreen)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            yaw += rotSpeed * Input.GetAxis("Mouse X");
            pitch -= rotSpeed * Input.GetAxis("Mouse Y");
            transform.eulerAngles = new Vector3(pitch, yaw, 0f);
        }
        else
        {
            transform.eulerAngles = new Vector3(pitch, yaw, 0f);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetKey(KeyCode.W))
        {
            myRigid.AddRelativeForce(new Vector3(0, 0, speed));
        }
        if (Input.GetKey(KeyCode.S))
        {
            myRigid.AddRelativeForce(new Vector3(0, 0, -speed));
        }
        if (Input.GetKey(KeyCode.A))
        {
            myRigid.AddRelativeForce(new Vector3(-speed, 0, 0));
        }
        if (Input.GetKey(KeyCode.D))
        {
            myRigid.AddRelativeForce(new Vector3(speed, 0, 0));
        }

        if(transform.position.y < 10)
        {
            transform.position = new Vector3(transform.position.x, 10f, transform.position.z);
        }
    }
}
