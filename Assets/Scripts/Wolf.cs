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
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        StartCoroutine("tick");
        StartCoroutine("incrementAge");
        StartCoroutine("checkCamera");
    }

    // Update is called once per frame
    void Update()
    {
        theUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "fox" && simSettings.foxPop.Count > 0 && simSettings.foxPop[simSettings.foxPop.Count - 1] > simSettings.rabbitPop[simSettings.rabbitPop.Count - 1] / 2f && simSettings.foxPop[simSettings.foxPop.Count - 1] > simSettings.wolfPop[simSettings.wolfPop.Count - 1] * 1.5f)
        {
            prey.Add(other.gameObject.transform.parent.gameObject);
        }
        if (other.tag == "rabbit" && simSettings.rabbitPop.Count > 0 && simSettings.rabbitPop[simSettings.rabbitPop.Count - 1] > simSettings.foxPop[simSettings.foxPop.Count - 1] * 2f && simSettings.rabbitPop[simSettings.rabbitPop.Count - 1] > simSettings.wolfPop[simSettings.wolfPop.Count - 1] * 3f)
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
        if (other.tag == "fox" || other.tag == "rabbit")
        {
            prey.Remove(other.gameObject.transform.parent.gameObject);
        }
        if (other.tag == "wolf")
        {
            selves.Remove(other.gameObject.transform.parent.gameObject);
        }
    }
}
