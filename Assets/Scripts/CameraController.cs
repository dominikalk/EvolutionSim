using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Rigidbody myRigid;

    public float speed = 100f;
    public float rotSpeed = 2;

    float yaw = 0f;
    float pitch = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        myRigid = GetComponent<Rigidbody>();
        Debug.Log(CalcPosition.findArrayPos(new Vector3(1,1,1)));
    }

    // Update is called once per frame
    void Update()
    {
        yaw += rotSpeed * Input.GetAxis("Mouse X");
        pitch -= rotSpeed * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch, yaw, 0f);

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
        if (transform.position.x < -256)
        {
            transform.position = new Vector3(-256f, transform.position.y, transform.position.z);
        }
        if (transform.position.x > 256)
        {
            transform.position = new Vector3(256f, transform.position.y, transform.position.z);
        }
        if (transform.position.z < -256)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -256f);
        }
        if (transform.position.z > 256)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 256f);
        }
    }
}
