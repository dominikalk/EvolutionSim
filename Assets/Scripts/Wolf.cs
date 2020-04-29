using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : Animal
{
    private bool canEatFox = true;
    private bool canEatRabbit = true;
    private bool foxesCanEat = true;

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
        gameObject.name = "Wolf";
    }

    // Update is called once per frame
    void Update()
    {
        theUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {
        checkCanEat();
        if (other.tag == "fox" && simSettings.foxPop.Count > 0 && foxesCanEat && canEatFox)
        {
            prey.Add(other.gameObject.transform.parent.gameObject);
        }
        if (other.tag == "rabbit" && simSettings.rabbitPop.Count > 0 && foxesCanEat && canEatRabbit)
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

    void checkCanEat()
    {
        if (simSettings.foxPop.Count > 0)
        {
            if (simSettings.wolfPop[simSettings.wolfPop.Count - 1] < simSettings.foxPop[simSettings.foxPop.Count - 1] / 1.25f)
            {
                canEatFox = true;
            }
            else if (simSettings.wolfPop[simSettings.wolfPop.Count - 1] > simSettings.foxPop[simSettings.foxPop.Count - 1] / 1.75f)
            {
                canEatFox = false;
            }

            if (simSettings.wolfPop[simSettings.wolfPop.Count - 1] < simSettings.rabbitPop[simSettings.rabbitPop.Count - 1] / 2.75f)
            {
                canEatRabbit = true;
            }
            else if (simSettings.wolfPop[simSettings.wolfPop.Count - 1] > simSettings.rabbitPop[simSettings.rabbitPop.Count - 1] / 3.25f)
            {
                canEatRabbit = false;
            }

            if (simSettings.foxPop[simSettings.foxPop.Count - 1] < simSettings.rabbitPop[simSettings.rabbitPop.Count - 1] / 1.75f)
            {
                foxesCanEat = true;
            }
            else if (simSettings.foxPop[simSettings.foxPop.Count - 1] > simSettings.rabbitPop[simSettings.rabbitPop.Count - 1] / 2.25f)
            {
                foxesCanEat = false;
            }
        }
    }
}
