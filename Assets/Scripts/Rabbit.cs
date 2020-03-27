using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Animal
{
    List<GameObject> plants;

    // Start is called before the first frame update
    void Start()
    {
        simSettings = FindObjectOfType<SimSettings>();
        plants = new List<GameObject>();
        setStatValues();
        GetComponent<SphereCollider>().radius = stat.range;

        StartCoroutine("tick");
        StartCoroutine("incrementAge");
    }

    IEnumerator tick()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            plants = checkExists(plants);
            if (plants.Count > 0)
            {
                float toX = plants[0].transform.position.x - 0.5f;
                float toY = simSettings.terrainSize - 0.5f - plants[0].transform.position.z;
                if(toX == xPos && toY == yPos)
                {
                    Destroy(plants[0]);
                    plants.RemoveAt(0);
                }
                moveTowards((int)toX, (int)toY);
            }
            else
            {
                randomMove();
            }
            Debug.Log(stat.maxHealth);
        }
    }

    // Update is called once per frame
    void Update()
    {
        checkDead();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "plant")
        {
            plants.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "plant")
        {
            plants.Remove(other.gameObject);
        }
    }
}
