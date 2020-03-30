using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Rigidbody myRigid;
    SimSettings simSettings;

    public float speed = 100f;
    public float rotSpeed = 2;

    float yaw = 0f;
    float pitch = 0f;

    // Start is called before the first frame update
    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
        transform.rotation = Quaternion.Euler(90, 0, 0);
        simSettings = FindObjectOfType<SimSettings>();
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
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            //transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
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
