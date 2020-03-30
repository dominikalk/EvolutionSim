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
        StartCoroutine("checkSustainable");
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
            prey.Add(other.gameObject);
        }
        if (other.tag == "fox")
        {
            selves.Add(other.gameObject);
        }
        if (other.tag == "wolf")
        {
            predators.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "rabbit")
        {
            prey.Remove(other.gameObject);
        }
        if (other.tag == "fox")
        {
            selves.Remove(other.gameObject);
        }
        if (other.tag == "wolf")
        {
            predators.Remove(other.gameObject);
        }
    }

    IEnumerator checkSustainable()
    {
        while (true)
        {
            yield return new WaitForSeconds(10);
            if (simSettings.rabbitPop < 100)
            {
                sustainableEating = false;
            }
            else
            {
                sustainableEating = true;
            }
        }
    }
}
