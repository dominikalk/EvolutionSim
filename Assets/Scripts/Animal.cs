using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat
{
    public float maxHealth;
    public float health;
    public float maxEnergy;
    public float energy;
    public float speed;
    public float size;
    public float rowdinessMultiplier;
    public float rowdiness;
    public float maxAge;
    public float age;
    public float range;
}

public class Animal : MonoBehaviour
{
    [SerializeField] GameObject selfObject;
    public List<GameObject> prey;
    public List<GameObject> selves;
    public List<GameObject> predators;

    public Stat stat = new Stat();
    public Stat[] parentStats;

    int prevXPos;
    int prevYPos;

    public int xPos;
    public int yPos;

    public bool isChild;
    public bool hasEaten = false;

    public SimSettings simSettings;
    [SerializeField] GameObject skull;
    [SerializeField] GameObject heart;

    bool moving;
    Vector3 startPos;
    Vector3 endPos;
    float trajectoryHeight = 1;
    float incrementor = 0;

    //TODO recheck height onsistancy

    public void theUpdate()
    {
        if (moving)
        {
            incrementor += (1f / stat.speed) * (1f / 0.5f) * Time.deltaTime;
            Vector3 currentPos = Vector3.Lerp(startPos, endPos, incrementor);
            if (gameObject.transform.Find("Trigger").gameObject.tag == "rabbit")
            {
                currentPos.y += trajectoryHeight * Mathf.Sin(Mathf.Clamp01(incrementor) * Mathf.PI);
            }
            gameObject.transform.position = currentPos;
            if (gameObject.transform.position == endPos)
            {
                moving = false;
                incrementor = 0;
            }
        }
    }

    public void setStatValues()
    {
        stat.speed = (parentStats[0].speed + parentStats[1].speed) / 2;
        stat.size = (parentStats[0].size + parentStats[1].size) / 2;
        stat.rowdinessMultiplier = (parentStats[0].rowdinessMultiplier + parentStats[1].rowdinessMultiplier) / 2;
        stat.maxAge = (parentStats[0].maxAge + parentStats[1].maxAge) / 2;
        stat.range = (parentStats[0].range + parentStats[1].range) / 2;

        evolveStats();

        stat.maxHealth = 100f * stat.size;
        stat.maxEnergy = 100f * stat.size;
        stat.health = stat.maxHealth;
        stat.energy = stat.maxEnergy;

        if (isChild)
        {
            stat.rowdiness = 0;
            stat.age = 0;
        }
        else
        {
            stat.rowdiness = Random.Range(0, 50);
            stat.age = Random.Range(0.0f, stat.maxAge);
        }
    }

    void evolveStats()
    {
        float randMultiplier = ((Random.Range(90, 111) / 100f) - 1f) * simSettings.evolMultplier + 1f;
        stat.speed *= randMultiplier;

        randMultiplier = ((Random.Range(90, 111) / 100f) - 1f) * simSettings.evolMultplier + 1f;
        stat.size *= randMultiplier;

        randMultiplier = ((Random.Range(90, 111) / 100f) - 1f) * simSettings.evolMultplier + 1f;
        stat.rowdinessMultiplier *= randMultiplier;

        randMultiplier = ((Random.Range(90, 111) / 100f) - 1f) * simSettings.evolMultplier + 1f;
        stat.maxAge *= randMultiplier;

        randMultiplier = ((Random.Range(90, 111) / 100f) - 1f) * simSettings.evolMultplier + 1f;
        stat.range *= randMultiplier;
    }

    public void chooseMove()
    {
        prey = checkExists(prey);
        selves = checkExists(selves);
        predators = checkExists(predators);
        bool moved = false;

        // Run Away From Closest Predator
        if (predators.Count > 0)
        {
            int index = findClosest(predators, false);
            Stat predatorStat = predators[index].GetComponent<Animal>().stat;
            if (predatorStat.energy < predatorStat.maxEnergy / 2f)
            {
                float toX = predators[index].transform.position.x - 0.5f;
                float toY = simSettings.terrainSize - 0.5f - predators[index].transform.position.z;
                moveTowards((int)toX, (int)toY, false);
                moved = true;
            }
        }

        // Reproduce With The Closest Same Animal
        if (stat.rowdiness >= 50 && hasEaten && selves.Count > 0 && !moved)
        {
            int index = findClosest(selves, true);
            if (selves[index].GetComponent<Animal>().hasEaten)
            {
                float toX = selves[index].transform.position.x - 0.5f;
                float toY = simSettings.terrainSize - 0.5f - selves[index].transform.position.z;
                moveTowards((int)toX, (int)toY, true);
                moved = true;

                int[,] surrounding = findSurrounding(xPos, yPos, false);
                for (int i = 0; i < 8; i++)
                {
                    if (surrounding[i, 0] == toX && surrounding[i, 1] == toY)
                    {
                        List<int[]> freeSurrounding = new List<int[]>();
                        for (int v = 0; v < 8; v++)
                        {
                            if (!simSettings.usedBlocks[surrounding[v, 0], surrounding[v, 1]] && simSettings.blockHeights[surrounding[v, 0], surrounding[v, 1]] >= 10)
                            {
                                freeSurrounding.Add(new int[] { surrounding[v, 0], surrounding[v, 1] });
                            }
                        }
                        int[] childPosition = randomPicker(freeSurrounding);
                        if (childPosition.Length > 0)
                        {
                            hasEaten = false;
                            selves[index].GetComponent<Animal>().hasEaten = false;
                            stat.rowdiness = 0;
                            //stat.energy -= stat.maxEnergy / 4f;
                            GameObject newHeart = Instantiate(heart, transform.position, Quaternion.identity);
                            newHeart.transform.parent = gameObject.transform.parent;
                            GameObject child = Instantiate(selfObject, new Vector3(childPosition[0] + 0.5f, simSettings.blockHeights[childPosition[0], childPosition[1]], simSettings.terrainSize - childPosition[1] - 0.5f), Quaternion.identity);
                            Animal script = child.GetComponent<Animal>();
                            script.xPos = childPosition[0];
                            script.yPos = childPosition[1];
                            script.parentStats = new Stat[]
                            {
                                stat,
                                selves[index].GetComponent<Animal>().stat
                            };
                            script.isChild = true;
                            simSettings.usedBlocks[childPosition[0], childPosition[1]] = true;
                            child.transform.parent = gameObject.transform.parent.transform;
                        }
                        break;
                    }
                }
            }
        }

        // Eat The Closest Food
        if (stat.energy < stat.maxEnergy / 2f && prey.Count > 0 && !moved)
        {
            int index = findClosest(prey, false);
            float toX = prey[index].transform.position.x - 0.5f;
            float toY = simSettings.terrainSize - 0.5f - prey[index].transform.position.z;
            moveTowards((int)toX, (int)toY, true);
            moved = true;
        }

        // Randomely Move
        if (!moved)
        {
            randomMove();
        }

        // Eat If Near Food
        if(stat.energy < stat.maxEnergy / 2f && prey.Count > 0)
        {
            int index = findClosest(prey, false);
            float toX = prey[index].transform.position.x - 0.5f;
            float toY = simSettings.terrainSize - 0.5f - prey[index].transform.position.z;

            int[,] surrounding = findSurrounding(xPos, yPos, false);
            for (int i = 0; i < 8; i++)
            {
                if (surrounding[i, 0] == toX && surrounding[i, 1] == toY)
                {
                    if (gameObject.transform.Find("Trigger").gameObject.tag == "rabbit")
                    {
                        prey[index].GetComponent<Plant>().eat();
                        simSettings.usedBlocks[(int)toX, (int)toY] = false;
                        prey.RemoveAt(index);
                        stat.energy += 20;
                        hasEaten = true;
                    }
                    else
                    {
                        Animal preyScript = prey[index].GetComponent<Animal>();
                        preyScript.stat.health -= stat.size * 20f;
                        if (preyScript.stat.health < 0)
                        {
                            //stat.energy = stat.maxEnergy;
                            stat.energy += preyScript.stat.energy;
                            preyScript.die();
                            prey.RemoveAt(index);
                            hasEaten = true;
                        }
                    }
                    if (stat.energy > stat.maxEnergy)
                    {
                        stat.energy = stat.maxEnergy;
                    }
                }
            }
        }
        checkStats();
    }

    void randomMove()
    {
        List<int[]> positions = new List<int[]>();
        int terrainSize = simSettings.terrainSize;
        int[,] surrounding = findSurrounding(xPos, yPos, false);
        int[,] prevSurrounding = findSurrounding(prevXPos, prevYPos, true);

        for (int i = 0; i < 8; i++)
        {
            if (surrounding[i, 0] > 0 && surrounding[i, 0] <= terrainSize && surrounding[i, 1] > 0 && surrounding[i, 1] <= terrainSize && simSettings.blockHeights[surrounding[i, 0], surrounding[i, 1]] >= 10)
            {
                if (!simSettings.usedBlocks[surrounding[i, 0], surrounding[i, 1]])
                {
                    bool isPrevious = false;
                    for (int v = 0; v < 9; v++)
                    {
                        if (surrounding[i, 0] == prevSurrounding[v, 0] && surrounding[i, 1] == prevSurrounding[v, 1])
                        {
                            isPrevious = true;
                        }
                    }
                    if (!isPrevious)
                    {
                        positions.Add(new int[] { surrounding[i, 0], surrounding[i, 1] });
                    }
                }
            }
        }

        int[] newPosition = randomPicker(positions);

        if(newPosition.Length > 0)
        {
            prevXPos = xPos;
            prevYPos = yPos;
            jumpTo(new Vector3(newPosition[0] + 0.5f, simSettings.blockHeights[newPosition[0], newPosition[1]], terrainSize - newPosition[1] - 0.5f));
            xPos = newPosition[0];
            yPos = newPosition[1];

            simSettings.usedBlocks[prevXPos, prevYPos] = false;
            simSettings.usedBlocks[xPos, yPos] = true;
        }
        else
        {
            prevXPos = -1;
            prevYPos = -1;
        }
    }

    void moveTowards(int toX, int toY, bool isTowards)
    {
        List<int[]> priority = new List<int[]>();
        int[,] surrounding = findSurrounding(xPos, yPos, false);
        List<int[]> notAdded = new List<int[]>();

        for (int i = 0; i < 8; i++)
        {
            notAdded.Add(new int[] { surrounding[i, 0], surrounding[i, 1] });
        }

        for (int i = 0; i < 8; i++)
        {
            float distance = Mathf.Infinity;
            if (!isTowards)
            {
                distance = Mathf.NegativeInfinity;
            }
            int index = 10;
            for (int v = 0; v < 8 - i; v++)
            {
                if (findDistance(notAdded[v][0], notAdded[v][1], toX, toY) < distance && isTowards)
                {
                    distance = findDistance(notAdded[v][0], notAdded[v][1], toX, toY);
                    index = v;
                }
                if (findDistance(notAdded[v][0], notAdded[v][1], toX, toY) > distance && !isTowards)
                {
                    distance = findDistance(notAdded[v][0], notAdded[v][1], toX, toY);
                    index = v;
                }
            }
            if (!simSettings.usedBlocks[notAdded[index][0], notAdded[index][1]] && simSettings.blockHeights[notAdded[index][0], notAdded[index][1]] >= 10)
            {
                priority.Add(notAdded[index]);
            }
            notAdded.RemoveAt(index);
        }

        if (priority.Count > 0)
        {
            jumpTo(new Vector3(priority[0][0] + 0.5f, simSettings.blockHeights[priority[0][0], priority[0][1]], simSettings.terrainSize - priority[0][1] - 0.5f));
            prevXPos = xPos;
            prevYPos = yPos;

            xPos = priority[0][0];
            yPos = priority[0][1];

            simSettings.usedBlocks[prevXPos, prevYPos] = false;
            simSettings.usedBlocks[xPos, yPos] = true;
        }
    }

    void jumpTo(Vector3 position)
    {
        float angle = Mathf.Atan2(position.x - gameObject.transform.position.x,position.z - gameObject.transform.position.z) * 180 / Mathf.PI;
        gameObject.transform.rotation = Quaternion.Euler(0, angle, 0);
        moving = true;
        startPos = gameObject.transform.position;
        endPos = position;
    }

    int[] randomPicker(List<int[]> positions)
    {
        if(positions.Count > 0)
        {
            return positions[Random.Range(0, positions.Count)];
        }
        else
        {
            return new int[0];
        }
    }

    int[,] findSurrounding(int x, int y, bool isPrev)
    {
        int[,] surroundingBlocks;
        // includes middle block
        if (isPrev)
        {
            surroundingBlocks = new int[,]
            {
                {x - 1, y - 1},
                {x, y - 1},
                {x + 1, y - 1},
                {x - 1, y},
                {x + 1, y},
                {x + 1, y + 1},
                {x, y + 1},
                {x - 1, y + 1},
                {x, y}
            };
        }
        //discludes middle block
        else
        {
            surroundingBlocks = new int[,]
            {
                {x - 1, y - 1},
                {x, y - 1},
                {x + 1, y - 1},
                {x + 1, y},
                {x + 1, y + 1},
                {x, y + 1},
                {x - 1, y + 1},
                {x - 1, y}
            };
        }
        return surroundingBlocks;
    }

    int findClosest(List<GameObject> theObjects, bool self)
    {
        float distance = Mathf.Infinity;
        int index = 0;
        for(int i = 0; i < theObjects.Count; i++)
        {
            if(findDistance(gameObject.transform.position.x, gameObject.transform.position.z, theObjects[i].transform.position.x, theObjects[i].transform.position.z) < distance)
            {
                if (self)
                {
                    if (theObjects[i].GetComponent<Animal>().hasEaten)
                    {
                        index = i;
                        distance = findDistance(gameObject.transform.position.x, gameObject.transform.position.z, theObjects[i].transform.position.x, theObjects[i].transform.position.z);
                    }
                }
                else
                {
                    index = i;
                    distance = findDistance(gameObject.transform.position.x, gameObject.transform.position.z, theObjects[i].transform.position.x, theObjects[i].transform.position.z);
                }
            }
        }
        return index;
    }

    float findDistance(float theXPos, float theYPos, float toX, float toY)
    {
        return Mathf.Sqrt((float)(toX - theXPos) * (float)(toX - theXPos) + (float)(toY - theYPos) * (float)(toY - theYPos));
    }

    List<GameObject> checkExists(List<GameObject> whatObjects)
    {
        List<GameObject> toRemoveList = new List<GameObject>();
        foreach (GameObject whatObject in whatObjects)
        {
            if (whatObject == null)
            {
                toRemoveList.Add(whatObject);
            }
        }
        foreach (GameObject toRemove in toRemoveList)
        {
            whatObjects.Remove(toRemove);
        }
        return whatObjects;
    }

    void checkStats()
    {
        stat.energy -= (1f / stat.size) + (stat.range / 10f);
        stat.rowdiness += stat.rowdinessMultiplier;
        if (stat.energy <= stat.maxEnergy / 5f)
        {
            stat.health -= 1;
        }
        else
        {
            stat.health += 2;
            if (stat.health > stat.maxHealth)
            {
                stat.health = stat.maxHealth;
            }
        }

        if (stat.age >= stat.maxAge)
        {
            die();
        }
        if(stat.energy <= 0)
        {
            die();
        }
        if (stat.health <= 0)
        {
            die();
        }
    }

    public void die()
    {
        GameObject newSkull = Instantiate(skull, gameObject.transform.position, Quaternion.identity);
        newSkull.transform.parent = gameObject.transform.parent;
        simSettings.usedBlocks[xPos, yPos] = false;
        Destroy(gameObject);
    }

    public IEnumerator tick()
    {
        while (true)
        {
            yield return new WaitForSeconds(stat.speed);
            chooseMove();
        }
    }

    public IEnumerator incrementAge()
    {
        while (true)
        {
            yield return new WaitForSeconds(10);
            stat.age += 0.1f;
        }
    }
}

/*
Movement:
 - animals need to know the block heights (average heights of the vertices) and the available blocks
    - best stored in a 2d array
 - I optimised and corrected the block height arrays
 - I got random movement working
 - next i need to make sure the animal doesnt back track
 - make sure they cant go off the edge
 - created a procedure to go to a specific position
 - for move towards i initially used the angle between the blocks, but then i switched to the distance for ease of priority selection



*/
