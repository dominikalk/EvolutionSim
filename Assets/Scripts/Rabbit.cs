using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Animal
{
    List<GameObject> plants;

    // Start is called before the first frame update
    void Start()
    {
        thisGameObject = this.gameObject;
        simSettings = FindObjectOfType<SimSettings>();
        plants = new List<GameObject>();
        xPos = 50;
        yPos = 50;

        StartCoroutine("tick");
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
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
