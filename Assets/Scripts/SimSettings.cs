using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public int stage = 0;
    bool stage1;
    bool stage2;
    bool stage3;
    bool stage4;
    bool stage5;

    public float rabbitPop;
    public float foxPop;
    public float wolfPop;

    public GameObject settingsPanel;
    public GameObject numbersPanel;
    public Text rabbitText;
    public Text foxText;
    public Text wolfText;
    public Text loadingText;
    public GameObject loadingPanel;
    public Slider loadingSlider;
    public GameObject pausePanel;
    public Slider timeSlider;

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
        speed = 0.75f,
        size = 2,
        rowdinessMultiplier = 20,
        maxAge = 5,
        range = 10
    };

    public Stat defaultWolfStat = new Stat()
    {
        speed = 0.6f,
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
        recordingGraph = "speed";
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
            Time.timeScale = 1;
            stage5 = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0;
            lockedScreen = false;
        }
    }

    IEnumerator checkAverage()
    {
        while (true)
        {
            yield return new WaitForSeconds(10);

            rabbitPop = FindObjectsOfType<Rabbit>().Length;
            foxPop = FindObjectsOfType<Fox>().Length;
            wolfPop = FindObjectsOfType<Wolf>().Length;
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
}
