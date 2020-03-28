using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Animal
{
    public GameObject rabbit;
    List<GameObject> plants;
    List<GameObject> rabbits;

    // Start is called before the first frame update
    void Start()
    {
        simSettings = FindObjectOfType<SimSettings>();
        plants = new List<GameObject>();
        rabbits = new List<GameObject>();
        setStatValues();
        GetComponent<SphereCollider>().radius = stat.range;

        StartCoroutine("tick");
        StartCoroutine("incrementAge");
    }

    IEnumerator tick()
    {
        while (true)
        {
            yield return new WaitForSeconds(stat.speed);
            plants = checkExists(plants);
            rabbits = checkExists(rabbits);
            bool moved = false;
            if(stat.energy < stat.maxEnergy / 2f)
            {
                if (plants.Count > 0)
                {
                    float toX = plants[0].transform.position.x - 0.5f;
                    float toY = simSettings.terrainSize - 0.5f - plants[0].transform.position.z;
                    moveTowards((int)toX, (int)toY);
                    moved = true;

                    int[,] surrounding = findSurrounding(xPos, yPos, false);
                    for (int i = 0; i < 8; i++)
                    {
                        if (surrounding[i, 0] == toX && surrounding[i, 1] == toY)
                        {
                            hasEaten = true;
                            Destroy(plants[0]);
                            plants.RemoveAt(0);
                            stat.energy += 20;
                            if (stat.energy > stat.maxEnergy)
                            {
                                stat.energy = stat.maxEnergy;
                            }
                        }
                    }
                }
            }
            if (stat.rowdiness >= 50 && stat.energy > stat.maxEnergy / 2f && hasEaten)
            {
                if(rabbits.Count > 0)
                {
                    float toX = rabbits[0].transform.position.x - 0.5f;
                    float toY = simSettings.terrainSize - 0.5f - rabbits[0].transform.position.z;
                    moveTowards((int)toX, (int)toY);
                    moved = true;

                    int[,] surrounding = findSurrounding(xPos, yPos, false);
                    for (int i = 0; i < 8; i++)
                    {
                        if (surrounding[i, 0] == toX && surrounding[i, 1] == toY)
                        {
                            List<int[]> freeSurrounding = new List<int[]>();
                            for(int v = 0; v < 8; v++)
                            {
                                if(!simSettings.usedBlocks[surrounding[v, 0], surrounding[v, 1]])
                                {
                                    freeSurrounding.Add(new int[] { surrounding[v, 0], surrounding[v, 1] });
                                }
                            }
                            int[] childPosition = randomPicker(freeSurrounding);
                            if(childPosition.Length > 0)
                            {
                                stat.rowdiness = 0;
                                stat.energy -= stat.maxEnergy / 4f;
                                GameObject newHeart = Instantiate(heart, transform.position, Quaternion.identity);
                                newHeart.transform.parent = gameObject.transform.parent;
                                GameObject childRabbit = Instantiate(rabbit, new Vector3(childPosition[0] + 0.5f, simSettings.blockHeights[childPosition[0], childPosition[1]], simSettings.terrainSize - childPosition[1] - 0.5f), Quaternion.identity);
                                Rabbit rabbitScript = childRabbit.GetComponent<Rabbit>();
                                rabbitScript.xPos = childPosition[0];
                                rabbitScript.yPos = childPosition[1];
                                rabbitScript.parentStats = new Stat[]
                                {
                                    stat,
                                    rabbits[0].GetComponent<Rabbit>().stat
                                };
                                rabbitScript.isChild = true;
                                simSettings.usedBlocks[childPosition[0], childPosition[1]] = true;
                                childRabbit.transform.parent = gameObject.transform.parent.transform;
                            }
                        }
                    }
                }
            }
            if (!moved)
            {
                randomMove();
            }
            checkStats();
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
        if (other.tag == "rabbit")
        {
            rabbits.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "plant")
        {
            plants.Remove(other.gameObject);
        }
        if (other.tag == "rabbit")
        {
            rabbits.Remove(other.gameObject);
        }
    }
}
