using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : Animal
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
        if (other.tag == "fox")
        {
            prey.Add(other.gameObject.transform.parent.gameObject);
        }
        if (other.tag == "wolf")
        {
            selves.Add(other.gameObject.transform.parent.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "fox")
        {
            prey.Remove(other.gameObject.transform.parent.gameObject);
        }
        if (other.tag == "wolf")
        {
            selves.Remove(other.gameObject.transform.parent.gameObject);
        }
    }
}
