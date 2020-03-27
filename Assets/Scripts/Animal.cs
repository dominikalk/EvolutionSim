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
    public float rowdiness;
    public float maxAge;
    public float age;
    public float range;
}

public class Animal : MonoBehaviour
{
    // Animal Statistic Structure
    public Stat stat = new Stat();
    public Stat[] parentStats;

    int prevXPos;
    int prevYPos;

    public int xPos;
    public int yPos;

    public SimSettings simSettings;

    public void setStatValues()
    {
        stat.maxHealth = (parentStats[0].maxHealth + parentStats[1].maxHealth) / 2;
        stat.maxEnergy = (parentStats[0].maxEnergy + parentStats[1].maxEnergy) / 2;
        stat.speed = (parentStats[0].speed + parentStats[1].speed) / 2;
        stat.size = (parentStats[0].size + parentStats[1].size) / 2;
        stat.rowdiness = (parentStats[0].rowdiness + parentStats[1].rowdiness) / 2;
        stat.maxAge = (parentStats[0].maxAge + parentStats[1].maxAge) / 2;
        stat.range = (parentStats[0].range + parentStats[1].range) / 2;

        evolveStats();

        stat.health = stat.maxHealth;
        stat.energy = stat.maxEnergy;
        stat.age = Random.Range(0.0f, stat.maxAge);
    }

    void evolveStats()
    {
        float randMultiplier = (Random.Range(90, 111) / 100f) * simSettings.evolMultplier;
        stat.maxHealth *= randMultiplier;

        randMultiplier = (Random.Range(90, 111) / 100f) * simSettings.evolMultplier;
        stat.maxEnergy *= randMultiplier;

        randMultiplier = (Random.Range(90, 111) / 100f) * simSettings.evolMultplier;
        stat.speed *= randMultiplier;

        randMultiplier = (Random.Range(90, 111) / 100f) * simSettings.evolMultplier;
        stat.size *= randMultiplier;

        randMultiplier = (Random.Range(90, 111) / 100f) * simSettings.evolMultplier;
        stat.rowdiness *= randMultiplier;

        randMultiplier = (Random.Range(90, 111) / 100f) * simSettings.evolMultplier;
        stat.maxAge *= randMultiplier;

        randMultiplier = (Random.Range(90, 111) / 100f) * simSettings.evolMultplier;
        stat.range *= randMultiplier;
    }

    public void randomMove()
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

        prevXPos = xPos;
        prevYPos = yPos;

        if(newPosition.Length > 0)
        {
            transform.position = new Vector3(newPosition[0] + 0.5f, simSettings.blockHeights[newPosition[0], newPosition[1]], terrainSize - newPosition[1] - 0.5f);
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

    public void moveTowards(int toX, int toY)
    {
        // Works out angle between positions
        float angle = Mathf.Atan2(yPos - toY, toX - xPos) * 180 / Mathf.PI;
        if (angle < 0)
        {
            angle = 360 + angle;
        }
        List<int[]> priority = new List<int[]>();
        int[,] surrounding = findSurrounding(xPos, yPos, false);

        // Picks the block closest to the position
        if (angle < 157.5 && angle >= 112.5)
        {
            priority.Add(new int[] { surrounding[0, 0], surrounding[0, 1] });
        }
        if (angle < 112.5 && angle >= 67.5)
        {
            priority.Add(new int[] { surrounding[1, 0], surrounding[1, 1] });
        }
        if (angle < 67.5 && angle >= 22.5)
        {
            priority.Add(new int[] { surrounding[2, 0], surrounding[2, 1] });
        }
        if (angle < 22.5 || angle >= 337.5)
        {
            priority.Add(new int[] { surrounding[3, 0], surrounding[3, 1] });
        }
        if (angle < 337.5 && angle >= 292.5)
        {
            priority.Add(new int[] { surrounding[4, 0], surrounding[4, 1] });
        }
        if (angle < 292.5 && angle >= 247.5)
        {
            priority.Add(new int[] { surrounding[5, 0], surrounding[5, 1] });
        }
        if (angle < 247.5 && angle >= 202.5)
        {
            priority.Add(new int[] { surrounding[6, 0], surrounding[6, 1] });
        }
        if (angle < 202.5 && angle >= 157.5)
        {
            priority.Add(new int[] { surrounding[7, 0], surrounding[7, 1] });
        }

        //TODO assign prority

        gameObject.transform.position = new Vector3(priority[0][0] + 0.5f, simSettings.blockHeights[priority[0][0], priority[0][1]], simSettings.terrainSize - priority[0][1] - 0.5f);
        prevXPos = xPos;
        prevYPos = yPos;

        xPos = priority[0][0];
        yPos = priority[0][1];

        simSettings.usedBlocks[prevXPos, prevYPos] = false;
        simSettings.usedBlocks[xPos, yPos] = true;
    }

    void jumpToPosition(Vector3 pos)
    {
        float desiredY = Mathf.Abs(pos.z - transform.position.z);
        Vector3 desiredPosition = new Vector3(pos.x, 1f + desiredY, pos.z);
        gameObject.transform.position = Vector3.MoveTowards(transform.position, desiredPosition, 20 * Time.deltaTime);
    }

    public List<GameObject> checkExists(List<GameObject> whatObjects)
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

    public void checkDead()
    {
        if (stat.age >= stat.maxAge)
        {
            Debug.Log("Dead");
            Destroy(gameObject);
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



*/
