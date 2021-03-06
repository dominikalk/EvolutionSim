﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Rigidbody myRigid;
    private SimSettings simSettings;

    private float speed = 150f;
    private float rotSpeed = 2;

    private float yaw = 0f;
    private float pitch = 0f;


    public bool isCinematic;
    private Vector3 initPosition;
    private Quaternion initRotation;
    private float cinematicSpeed = 1;
    private float journeyLength = 4;
    private float distCovered;
    [SerializeField] private GameObject cinematicPosition;


    // Start is called before the first frame update
    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
        simSettings = FindObjectOfType<SimSettings>();
        pitch = transform.eulerAngles.x;
        yaw = transform.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isCinematic)
            {
                isCinematic = false;
                pitch = transform.eulerAngles.x;
                yaw = transform.eulerAngles.y;
                simSettings.lockedScreen = true;
            }
            else
            {
                isCinematic = true;
                distCovered = 0;
                initPosition = transform.position;
                initRotation = transform.rotation;
                simSettings.lockedScreen = false;
            }
        }

        if(simSettings.lockedScreen)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            if (!isCinematic)
            {
                yaw += rotSpeed * Input.GetAxis("Mouse X");
                pitch -= rotSpeed * Input.GetAxis("Mouse Y");
                transform.eulerAngles = new Vector3(pitch, yaw, 0f);
            }
        }
        else
        {
            transform.eulerAngles = new Vector3(pitch, yaw, 0f);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if(Time.timeScale > 0 && isCinematic)
        {
            distCovered += Time.deltaTime * (cinematicSpeed / Time.timeScale);
            float fractionOfJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(initPosition, cinematicPosition.transform.position, fractionOfJourney);
            transform.rotation = Quaternion.Lerp(initRotation, cinematicPosition.transform.rotation, fractionOfJourney);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0f);
            pitch = transform.eulerAngles.x;
            yaw = transform.eulerAngles.y;
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "plant")
        {
            MeshRenderer[] meshRenderers = other.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer mesh in meshRenderers)
            {
                mesh.enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "plant")
        {
            if (other.tag == "plant")
            {
                MeshRenderer[] meshRenderers = other.GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer mesh in meshRenderers)
                {
                    mesh.enabled = false;
                }
            }
        }
    }
}
