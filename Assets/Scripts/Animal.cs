using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    // Animal Statistic Structure
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
        public int age;
    }
    public Stat stat = new Stat();
    public Stat[] parentStats = new Stat[2];

    int prevXPos;
    int prevYPos;

    int xPos = 0;
    int yPos = 0;

    public SimSettings simSettings;
    public GameObject thisGameObject;


    // Start is called before the first frame update
    void Start()
    {
        setStatValues();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void setStatValues()
    {
        stat.maxHealth = (parentStats[0].maxHealth + parentStats[0].maxHealth) / 2;
        stat.maxEnergy = (parentStats[0].maxEnergy + parentStats[0].maxEnergy) / 2;
        stat.speed = (parentStats[0].speed + parentStats[0].speed) / 2;
        stat.size = (parentStats[0].size + parentStats[0].size) / 2;
        stat.rowdiness = (parentStats[0].rowdiness + parentStats[0].rowdiness) / 2;
        stat.maxAge = (parentStats[0].maxAge + parentStats[0].maxAge) / 2;

        evolveStats();

        stat.health = stat.maxHealth;
        stat.energy = stat.maxEnergy;
        stat.age = 0;
    }

    void evolveStats()
    {
        float randMultiplier = (Random.Range(90, 111) / 100) * simSettings.evolMultplier;
        stat.maxHealth *= randMultiplier;

        randMultiplier = (Random.Range(90, 111) / 100) * simSettings.evolMultplier;
        stat.maxEnergy *= randMultiplier;

        randMultiplier = (Random.Range(90, 111) / 100) * simSettings.evolMultplier;
        stat.speed *= randMultiplier;

        randMultiplier = (Random.Range(90, 111) / 100) * simSettings.evolMultplier;
        stat.size *= randMultiplier;

        randMultiplier = (Random.Range(90, 111) / 100) * simSettings.evolMultplier;
        stat.rowdiness *= randMultiplier;

        randMultiplier = (Random.Range(90, 111) / 100) * simSettings.evolMultplier;
        stat.maxAge *= randMultiplier;
    }

    public IEnumerator tick()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            randomMove();
        }
    }

    void randomMove()
    {
        List<int[]> positions = new List<int[]>();
        int terrainSize = simSettings.terrainSize;
        int[,] surrounding = findSurrounding(xPos, yPos, false);
        int[,] prevSurrounding = findSurrounding(prevXPos, prevYPos, true);

        for (int i = 0; i < 8; i++)
        {
            if (surrounding[i, 0] > 0 && surrounding[i, 0] <= terrainSize && surrounding[i, 1] > 0 && surrounding[i, 1] <= terrainSize)
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
        }
        else
        {
            prevXPos = Random.Range(0, terrainSize + 1);
            prevYPos = Random.Range(0, terrainSize + 1);
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
                {x + 1, y + 1},
                {x, y}
            };
        }
        else
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
                {x + 1, y + 1}
            };
        }
        return surroundingBlocks;
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



*/
