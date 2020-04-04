using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

using System.Linq;

public class Graph : MonoBehaviour
{
    RectTransform graphContainer;
    [SerializeField] Sprite circleSprite;
    RectTransform labelTemplateX;
    RectTransform labelTemplateY;
    RectTransform dashTemplateX;
    RectTransform dashTemplateY;
    GameObject toolTip;
    GameObject notEnough;
    public List<float> rabbitList = new List<float>();
    public List<float> foxList = new List<float>();
    public List<float> wolfList = new List<float>();

    // Start is called before the first frame update
    void Awake()
    {
        graphContainer = transform.Find("GraphContainer").GetComponent<RectTransform>();
        labelTemplateX = graphContainer.Find("LabelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("LabelTemplateY").GetComponent<RectTransform>();
        dashTemplateX = graphContainer.Find("DashTemplateX").GetComponent<RectTransform>();
        dashTemplateY = graphContainer.Find("DashTemplateY").GetComponent<RectTransform>();
        toolTip = graphContainer.Find("ToolTip").gameObject;
        notEnough = graphContainer.Find("NotEnough").gameObject;
    }

    IEnumerator addGraphValue()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1);

            rabbitList.Add(Random.Range(0, 101));
            foxList.Add(Random.Range(0, 101));
            wolfList.Add(Random.Range(0, 101));
            ShowGraph();
        }
    }

    void clear()
    {
        Transform[] targets = graphContainer.GetComponentsInChildren<Transform>();
        for(int i = 0; i < targets.Length; i++)
        {
            if (targets[i].name == "circle" || targets[i].name == "DashTemplateX(Clone)" || targets[i].name == "DashTemplateY(Clone)" || targets[i].name == "dotConnection" || targets[i].name == "LabelTemplateY(Clone)" || targets[i].name == "LabelTemplateX(Clone)")
            {
                Destroy(targets[i].gameObject);
            }
        }
    }

    public void ShowToolTip(string text, Vector2 pos)
    {
        toolTip.SetActive(true);
        
        toolTip.GetComponent<RectTransform>().anchoredPosition = new Vector2(pos.x + 4f, pos.y + 4f);
        Text toolTipText = toolTip.transform.Find("Text").GetComponent<Text>();
        toolTipText.text = text;
        float padding = 4f;
        Vector2 backgroundSize = new Vector2(
            toolTipText.preferredWidth + padding * 2f,
            toolTipText.preferredHeight + padding * 2f
        );
        toolTip.transform.Find("Background").GetComponent<RectTransform>().sizeDelta = backgroundSize;
        toolTip.transform.SetAsLastSibling();
    }

    public void HideToolTip()
    {
        toolTip.SetActive(false);
    }

    GameObject CreateCircle(Vector2 position)
    {
        GameObject circle = new GameObject("circle", typeof(Image));
        circle.transform.SetParent(graphContainer, false);
        circle.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = circle.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = position;
        rectTransform.sizeDelta = new Vector2(7, 7);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return circle;
    }

    public void ShowGraph()
    {
        clear();
        if(rabbitList.Count > 1)
        {
            notEnough.SetActive(false);
            float graphHeight = graphContainer.sizeDelta.y;
            float graphWidth = graphContainer.sizeDelta.x;
            float yMax = Mathf.NegativeInfinity;
            float xSize = graphWidth / (rabbitList.Count - 1f);
            GameObject lastCircleRabbit = null;
            GameObject lastCircleFox = null;
            GameObject lastCircleWolf = null;

            for (int i = 0; i < rabbitList.Count; i++)
            {
                if (rabbitList[i] > yMax)
                {
                    yMax = rabbitList[i];
                }
                if (foxList[i] > yMax)
                {
                    yMax = foxList[i];
                }
                if (wolfList[i] > yMax)
                {
                    yMax = wolfList[i];
                }
            }

            float skipping = Mathf.Ceil(rabbitList.Count / 10f);

            for (int i = 0; i < rabbitList.Count; i++)
            {
                if (i % skipping == 0f)
                {
                    float xPos = i * xSize;

                    CreateLineGraph(rabbitList, i, xSize, xPos, yMax, graphHeight, ref lastCircleRabbit, new Color(0.71f, 0.4f, 0.11f));
                    CreateLineGraph(foxList, i, xSize, xPos, yMax, graphHeight, ref lastCircleFox, new Color(1, 0.27f, 0f));
                    CreateLineGraph(wolfList, i, xSize, xPos, yMax, graphHeight, ref lastCircleWolf, Color.gray);

                    RectTransform labelX = Instantiate(labelTemplateX);
                    labelX.SetParent(graphContainer);
                    labelX.gameObject.SetActive(true);
                    labelX.anchoredPosition = new Vector2(xPos, -3f);
                    labelX.GetComponent<Text>().text = "Day " + i.ToString();
                    labelX.localScale = new Vector3(1, 1, 1);

                    RectTransform dashX = Instantiate(dashTemplateY);
                    dashX.SetParent(graphContainer);
                    dashX.SetAsFirstSibling();
                    dashX.gameObject.SetActive(true);
                    dashX.anchoredPosition = new Vector2(xPos, 0f);
                    dashX.localScale = new Vector3(1f, 1f, 1f);
                    dashX.GetComponent<RectTransform>().sizeDelta = new Vector2(graphHeight, 1);
                }
            }

            int separatorCount = 10;
            for (int i = 0; i <= separatorCount; i++)
            {
                RectTransform labelY = Instantiate(labelTemplateY);
                labelY.SetParent(graphContainer);
                labelY.gameObject.SetActive(true);
                float yHeightNorm = ((float)i / separatorCount);
                labelY.anchoredPosition = new Vector2(-3f, yHeightNorm * graphHeight);
                labelY.GetComponent<Text>().text = (roundToSF(yHeightNorm * yMax, 3)).ToString();
                labelY.localScale = new Vector3(1, 1, 1);

                RectTransform dashY = Instantiate(dashTemplateX);
                dashY.SetParent(graphContainer);
                dashY.SetAsFirstSibling();
                dashY.gameObject.SetActive(true);
                dashY.anchoredPosition = new Vector2(0f, yHeightNorm * graphHeight);
                dashY.localScale = new Vector3(1f, 1f, 1f);
                dashY.GetComponent<RectTransform>().sizeDelta = new Vector2(graphWidth, 1);
            }
            graphContainer.transform.Find("Background").SetAsFirstSibling();
        }
        else
        {
            notEnough.SetActive(true);
        }
    }

    void CreateLineGraph(List<float> popList, int i, float xSize, float xPos, float yMax, float graphHeight, ref GameObject lastCircleGameObject, Color color)
    {
        float yPos = (popList[i] / yMax) * graphHeight;
        float textHeight = popList[i];
        GameObject circleGameObject = CreateCircle(new Vector2(xPos, yPos));
        circleGameObject.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 1f);

        EventTrigger.Entry mouseOver = new EventTrigger.Entry();
        mouseOver.eventID = EventTriggerType.PointerEnter;
        mouseOver.callback.RemoveAllListeners();
        mouseOver.callback.AddListener((eventData) => { ShowToolTip(roundToSF(textHeight, 3).ToString(), new Vector2(xPos, yPos)); });

        EventTrigger.Entry mouseExit = new EventTrigger.Entry();
        mouseExit.eventID = EventTriggerType.PointerExit;
        mouseExit.callback.RemoveAllListeners();
        mouseExit.callback.AddListener((eventData) => { HideToolTip(); });

        circleGameObject.gameObject.AddComponent<EventTrigger>().triggers.Add(mouseOver);
        circleGameObject.gameObject.GetComponent<EventTrigger>().triggers.Add(mouseExit);

        if (lastCircleGameObject != null)
        {
            GameObject line = CreateLine(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            line.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 0.5f);
        }
        lastCircleGameObject = circleGameObject;
    }

    GameObject CreateLine(Vector2 posA, Vector2 posB)
    {
        GameObject line = new GameObject("dotConnection", typeof(Image));
        line.transform.SetParent(graphContainer, false);
        line.transform.SetAsFirstSibling();
        line.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
        RectTransform rectTransform = line.GetComponent<RectTransform>();
        Vector2 dirVector = (posB - posA).normalized;
        float dirAngle = Mathf.Rad2Deg * Mathf.Atan2(dirVector.y, dirVector.x);
        float distance = Vector2.Distance(posB, posA);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = posA + dirVector * distance * 0.5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, dirAngle);
        return line;
    }

    public void populationClicked()
    {
        SimSettings simSettings = FindObjectOfType<SimSettings>();
        rabbitList = simSettings.rabbitPop;
        foxList = simSettings.foxPop;
        wolfList = simSettings.wolfPop;
        ShowGraph();
    }

    public void optionClicked()
    {
        SimSettings simSettings = FindObjectOfType<SimSettings>();
        rabbitList = simSettings.rabbitOption;
        foxList = simSettings.foxOption;
        wolfList = simSettings.wolfOption;
        ShowGraph();
    }

    float roundToSF(float d, int digits)
    {
        if (d == 0)
        {
            return 0;
        }

        float scale = (float)Mathf.Pow(10, Mathf.Floor(Mathf.Log10(Mathf.Abs(d))) + 1);
        return scale * (Mathf.Round(d / scale * Mathf.Pow(10f, digits)) / Mathf.Pow(10f, digits));
    }
}
