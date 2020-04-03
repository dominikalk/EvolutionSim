using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox : Animal
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
        if (other.tag == "rabbit")
        {
            prey.Add(other.gameObject.transform.parent.gameObject);
        }
        if (other.tag == "fox")
        {
            selves.Add(other.gameObject.transform.parent.gameObject);
        }
        if (other.tag == "wolf")
        {
            predators.Add(other.gameObject.transform.parent.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "rabbit")
        {
            prey.Remove(other.gameObject.transform.parent.gameObject);
        }
        if (other.tag == "fox")
        {
            selves.Remove(other.gameObject.transform.parent.gameObject);
        }
        if (other.tag == "wolf")
        {
            predators.Remove(other.gameObject.transform.parent.gameObject);
        }
    }
}
