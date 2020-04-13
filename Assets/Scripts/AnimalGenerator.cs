using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalGenerator : MonoBehaviour
{
    [SerializeField] private GameObject rabbit;
    [SerializeField] private GameObject fox;
    [SerializeField] private GameObject wolf;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateAnimals()
    {
        SimSettings simSettings = FindObjectOfType<SimSettings>();
        Stat[] rabbitParentStats = new Stat[] 
        {
            simSettings.defaultRabbitStat,
            simSettings.defaultRabbitStat
        };
        Stat[] foxParentStats = new Stat[]
        {
            simSettings.defaultFoxStat,
            simSettings.defaultFoxStat
        };
        Stat[] wolfParentStats = new Stat[]
        {
            simSettings.defaultWolfStat,
            simSettings.defaultWolfStat
        };

        int rabbitsLeft = simSettings.rabbits;
        int foxesLeft = simSettings.foxes;
        int wolvesLeft = simSettings.wolves;
        int xPos;
        int yPos;

        while (true)
        {
            if (rabbitsLeft == 0)
            {
                break;
            }
            xPos = Random.Range(0, simSettings.terrainSize);
            yPos = Random.Range(0, simSettings.terrainSize);
            if(!simSettings.usedBlocks[xPos, yPos] && simSettings.blockHeights[xPos, yPos] >= 11)
            {
                GameObject newAnimal = Instantiate(rabbit, new Vector3(xPos + 0.5f, simSettings.blockHeights[xPos, yPos], simSettings.terrainSize - yPos - 0.5f), Quaternion.identity);
                Animal script = newAnimal.GetComponent<Animal>();
                script.xPos = xPos;
                script.yPos = yPos;
                script.parentStats = rabbitParentStats;
                simSettings.usedBlocks[xPos, yPos] = true;
                newAnimal.transform.parent = gameObject.transform;
                rabbitsLeft -= 1;
            }
        }
        while (true)
        {
            if (foxesLeft == 0)
            {
                break;
            }
            xPos = Random.Range(0, simSettings.terrainSize);
            yPos = Random.Range(0, simSettings.terrainSize);
            if (!simSettings.usedBlocks[xPos, yPos] && simSettings.blockHeights[xPos, yPos] >= 11)
            {
                GameObject newAnimal = Instantiate(fox, new Vector3(xPos + 0.5f, simSettings.blockHeights[xPos, yPos], simSettings.terrainSize - yPos - 0.5f), Quaternion.identity);
                Animal script = newAnimal.GetComponent<Animal>();
                script.xPos = xPos;
                script.yPos = yPos;
                script.parentStats = foxParentStats;
                simSettings.usedBlocks[xPos, yPos] = true;
                newAnimal.transform.parent = gameObject.transform;
                foxesLeft -= 1;
            }
        }
        while (true)
        {
            if (wolvesLeft == 0)
            {
                break;
            }
            xPos = Random.Range(0, simSettings.terrainSize);
            yPos = Random.Range(0, simSettings.terrainSize);
            if (!simSettings.usedBlocks[xPos, yPos] && simSettings.blockHeights[xPos, yPos] >= 11)
            {
                GameObject newAnimal = Instantiate(wolf, new Vector3(xPos + 0.5f, simSettings.blockHeights[xPos, yPos], simSettings.terrainSize - yPos - 0.5f), Quaternion.identity);
                Animal script = newAnimal.GetComponent<Animal>();
                script.xPos = xPos;
                script.yPos = yPos;
                script.parentStats = wolfParentStats;
                simSettings.usedBlocks[xPos, yPos] = true;
                newAnimal.transform.parent = gameObject.transform;
                wolvesLeft -= 1;
            }
        }
        simSettings.stage += 1;
    }
}
