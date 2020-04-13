using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject instPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startClicked()
    {
        SceneManager.LoadScene("Simulation");
    }

    public void quitClicked()
    {
        Application.Quit();
    }

    public void infoEnter(bool isInst)
    {
        if (isInst)
        {
            instPanel.SetActive(true);
        }
        else
        {
            infoPanel.SetActive(true);
        }
    }

    public void infoExit(bool isInst)
    {
        if(isInst)
        {
            instPanel.SetActive(false);
        }
        else
        {
            infoPanel.SetActive(false);
        }
    }
}
