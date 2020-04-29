using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox : Animal
{
    private bool canEat = true;

    // Start is called before the first frame update
    void Start()
    {
        simSettings = FindObjectOfType<SimSettings>();

        prey = new List<GameObject>();
        selves = new List<GameObject>();
        predators = new List<GameObject>();

        setStatValues();
        GetComponent<SphereCollider>().radius = stat.range;
        rb = GetComponent<Rigidbody>();

        StartCoroutine("tick");
        StartCoroutine("incrementAge");
        gameObject.name = "Fox";
    }

    // Update is called once per frame
    void Update()
    {
        theUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {
        checkCanEat();
        if (other.tag == "rabbit" && canEat)
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

    void checkCanEat()
    {
        if (simSettings.rabbitPop.Count > 0)
        {
            if (simSettings.foxPop[simSettings.foxPop.Count - 1] < simSettings.rabbitPop[simSettings.rabbitPop.Count - 1] / 1.75f)
            {
                canEat = true;
            }
            else if (simSettings.foxPop[simSettings.foxPop.Count - 1] > simSettings.rabbitPop[simSettings.rabbitPop.Count - 1] / 2.25f)
            {
                canEat = false;
            }
        }
    }
}
