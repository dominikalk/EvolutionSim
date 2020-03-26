using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Animal
{
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(stat.age);
        thisGameObject = this.gameObject;
        simSettings = FindObjectOfType<SimSettings>();
        StartCoroutine("tick");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
