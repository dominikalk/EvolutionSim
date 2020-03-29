using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Animal
{
    // Start is called before the first frame update
    void Start()
    {
        simSettings = FindObjectOfType<SimSettings>();

        prey = new List<GameObject>();
        selves = new List<GameObject>();
        predators = new List<GameObject>();

        setStatValues();
        GetComponent<SphereCollider>().radius = stat.range;

        StartCoroutine("tick");
        StartCoroutine("incrementAge");
    }

    // Update is called once per frame
    void Update()
    {
        theUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "plant")
        {
            prey.Add(other.gameObject);
        }
        if (other.tag == "rabbit")
        {
            selves.Add(other.gameObject);
        }
        if (other.tag == "wolf" || other.tag == "fox")
        {
            predators.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "plant")
        {
            prey.Remove(other.gameObject);
        }
        if (other.tag == "rabbit")
        {
            selves.Remove(other.gameObject);
        }
        if (other.tag == "wolf" || other.tag == "fox")
        {
            predators.Remove(other.gameObject);
        }
    }
}
