using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicCamera : MonoBehaviour
{
    float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale != 0)
        {
            transform.Rotate(0, (speed * Time.deltaTime) / Time.timeScale, 0);
        }
    }
}
