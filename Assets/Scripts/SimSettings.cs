using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SimSettings: MonoBehaviour
{
    // User Set Properties
    public int terrainSize;
    public int objectThickness;
    public float evolMultplier;
    public int rabbits;
    public int foxes;
    public int wolves;
    public string recordingGraph;
    public int renderQuality;

    // Other Properties
    public int objectOffset;
    public bool[,] usedBlocks;
    public float[,] blockHeights;
    public int stage;
    bool stage1;
    bool stage2;
    bool stage3;
    bool stage4;
    bool stage5;

    public List<float> rabbitPop;
    public List<float> foxPop;
    public List<float> wolfPop;

    public List<float> rabbitOption;
    public List<float> foxOption;
    public List<float> wolfOption;

    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject numbersPanel;
    [SerializeField] Text rabbitText;
    [SerializeField] Text foxText;
    [SerializeField] Text wolfText;
    [SerializeField] Text loadingText;
    [SerializeField] GameObject loadingPanel;
    [SerializeField] Slider loadingSlider;
    [SerializeField] GameObject pausePanel;
    [SerializeField] Slider timeSlider;

    [SerializeField] GameObject graph;
    [SerializeField] Text optionText;
    [SerializeField] Toggle populationButton;

    public bool lockedScreen;

    public Stat defaultRabbitStat = new Stat()
    {
        speed = 1,
        size = 1,
        rowdinessMultiplier = 10,
        maxAge = 2,
        range = 4
    };

    public Stat defaultFoxStat = new Stat()
    {
        speed = 0.5f,
        size = 2,
        rowdinessMultiplier = 20,
        maxAge = 5,
        range = 10
    };

    public Stat defaultWolfStat = new Stat()
    {
        speed = 0.75f,
        size = 3,
        rowdinessMultiplier = 20,
        maxAge = 8,
        range = 15
    };

    // Start is called before the first frame update
    void Start()
    {
        objectOffset = Random.Range(0, 100);
        Time.timeScale = 0;
        StartCoroutine("checkAverage");
        stage = 0;
        terrainSize = 128;
        objectThickness = 20;
        evolMultplier = 1;
        recordingGraph = "Speed";
        renderQuality = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if(stage == 1 && !stage1)
        {
            settingsPanel.SetActive(false);
            loadingPanel.SetActive(true);
            loadingSlider.value = 1;
            loadingText.text = "Generating Terrain ...";
            FindObjectOfType<TerrainGenerator>().GeneratePlane();
            stage1 = true;
        }
        else if (stage == 2 && !stage2)
        {
            loadingSlider.value = 2;
            loadingText.text = "Generating Forest ...";
            FindObjectOfType<ObjectGenerator>().GenerateObjects();
            stage2 = true;
        }
        else if (stage == 3 && !stage3)
        {
            loadingSlider.value = 3;
            loadingText.text = "Generating Plants ...";
            FindObjectOfType<PlantGenerator>().GeneratePlants();
            stage3 = true;
        }
        else  if(stage == 4 && !stage4)
        {
            loadingSlider.value = 4;
            loadingText.text = "Generating Animals ...";
            FindObjectOfType<AnimalGenerator>().GenerateAnimals();
            stage4 = true;
        }
        else if (stage == 5 && !stage5)
        {
            lockedScreen = true;
            loadingPanel.SetActive(false);
            optionText.text = recordingGraph;
            Time.timeScale = 1;
            stage5 = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && stage == 5)
        {
            pausePanel.SetActive(true);
            populationButton.isOn = true;
            Graph graphScript = graph.GetComponent<Graph>();
            graphScript.rabbitList = rabbitPop;
            graphScript.foxList = foxPop;
            graphScript.wolfList = wolfPop;
            graphScript.ShowGraph();
            Time.timeScale = 0;
            lockedScreen = false;
        }
    }

    IEnumerator checkAverage()
    {
        while (true)
        {
            yield return new WaitForSeconds(10);

            Rabbit[] rabbitScripts = FindObjectsOfType<Rabbit>();
            Fox[] foxScripts = FindObjectsOfType<Fox>();
            Wolf[] wolfScripts = FindObjectsOfType<Wolf>();

            rabbitPop.Add(rabbitScripts.Length);
            foxPop.Add(foxScripts.Length);
            wolfPop.Add(wolfScripts.Length);

            float rabbitOptionAverage = 0;
            float foxOptionAverage = 0;
            float wolfOptionAverage = 0;

            for (int i = 0; i < rabbitScripts.Length; i++)
            {
                switch (recordingGraph)
                {
                    case "Speed":
                        rabbitOptionAverage += rabbitScripts[i].stat.speed;
                        break;
                    case "Size":
                        rabbitOptionAverage += rabbitScripts[i].stat.size;
                        break;
                    case "Life Expectancy":
                        rabbitOptionAverage += rabbitScripts[i].stat.maxAge;
                        break;
                    case "Range":
                        rabbitOptionAverage += rabbitScripts[i].stat.range;
                        break;
                }
            }
            rabbitOptionAverage /= rabbitScripts.Length;

            for (int i = 0; i < foxScripts.Length; i++)
            {
                switch (recordingGraph)
                {
                    case "Speed":
                        foxOptionAverage += foxScripts[i].stat.speed;
                        break;
                    case "Size":
                        foxOptionAverage += foxScripts[i].stat.size;
                        break;
                    case "Life Expectancy":
                        foxOptionAverage += foxScripts[i].stat.maxAge;
                        break;
                    case "Range":
                        foxOptionAverage += foxScripts[i].stat.range;
                        break;
                }
            }
            foxOptionAverage /= foxScripts.Length;

            for (int i = 0; i < wolfScripts.Length; i++)
            {
                switch (recordingGraph)
                {
                    case "Speed":
                        wolfOptionAverage += wolfScripts[i].stat.speed;
                        break;
                    case "Size":
                        wolfOptionAverage += wolfScripts[i].stat.size;
                        break;
                    case "Life Expectancy":
                        wolfOptionAverage += wolfScripts[i].stat.maxAge;
                        break;
                    case "Range":
                        wolfOptionAverage += wolfScripts[i].stat.range;
                        break;
                }
            }
            wolfOptionAverage /= wolfScripts.Length;

            switch (recordingGraph)
            {
                case "Speed":
                    rabbitOption.Add((1f / rabbitOptionAverage) * 10f);
                    foxOption.Add((1f / foxOptionAverage) * 10f);
                    wolfOption.Add((1f / wolfOptionAverage) * 10f);
                    break;
                case "Size":
                    rabbitOption.Add(rabbitOptionAverage * 10f);
                    foxOption.Add(foxOptionAverage * 10f);
                    wolfOption.Add(wolfOptionAverage * 10f);
                    break;
                default:
                    rabbitOption.Add(rabbitOptionAverage);
                    foxOption.Add(foxOptionAverage);
                    wolfOption.Add(wolfOptionAverage);
                    break;
            }
        }
    }

    public void gererateClicked()
    {
        try
        {
            rabbits = int.Parse(rabbitText.text);
            foxes = int.Parse(foxText.text);
            wolves = int.Parse(wolfText.text);
            settingsPanel.SetActive(false);
            QualitySettings.SetQualityLevel(renderQuality, true);
            stage = 1;
        }
        catch
        {
            numbersPanel.SetActive(true);
        }
    }

    public void terrainClicked(int theTerrainSize)
    {
        terrainSize = theTerrainSize;
    }

    public void forsetClicked(int forestDensity)
    {
        objectThickness = forestDensity;
    }

    public void evolutionClicked(float evolution)
    {
        evolMultplier = evolution;
    }

    public void renderClicked(int qualityLevel)
    {
        renderQuality = qualityLevel;
    }

    public void numbersClicked()
    {
        numbersPanel.SetActive(false);
    }

    public void graphClicked(string graph)
    {
        recordingGraph = graph;
    }

    public void resumeClicked()
    {
        Time.timeScale = timeSlider.value;
        pausePanel.SetActive(false);
        lockedScreen = true;
    }

    public void resetSimulation()
    {
        SceneManager.LoadScene("Simulation");
    }

    public void menuClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

/*
 Issues:
  - float was too large so gave infinity so ui hade to have an extra switch case statement
     
     
     */
