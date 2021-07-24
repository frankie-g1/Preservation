using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class windowGraph : MonoBehaviour
{
    [SerializeField] private Sprite circleSprite;
    private RectTransform graphContainer;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;
    private RectTransform dashTemplateX;
    private RectTransform dashTemplateY;
    


    private void Awake(){
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        labelTemplateX = graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("labelTemplateY").GetComponent<RectTransform>();
        dashTemplateX = graphContainer.Find("dashTemplateX").GetComponent<RectTransform>();
        dashTemplateY = graphContainer.Find("dashTemplateY").GetComponent<RectTransform>();
        //ValueList will be fed infection rates 
        List<int> valueList = new List<int>() {500, 9800, 5600, 4500, 3000, 2002, 1700, 1500, 9001, 1700, 2500, 3700, 4000, 3600, 3300};
        //infected = GameManager.getInstance().get
        
        //Can get rid of, but it adds labels
        ShowGraph(valueList, (int _i) => "Day " + (_i + 1), (float _f) => "$" + Mathf.RoundToInt(_f));
    }

    private GameObject CreateCircle(Vector2 anchoredPosition){
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(2, 2);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

    private void ShowGraph(List<int> valueList, Func <int, string> getAxisLabelX, Func <float, string> getAxisLabelY = null){ //List of all the values we want to graph
        
        if(getAxisLabelX == null){
            getAxisLabelX = delegate (int _i) { return _i.ToString();};
        }

        if(getAxisLabelY == null){
            getAxisLabelY = delegate (float _f) { return Mathf.RoundToInt(_f).ToString();};
        }
        
        
        float xSize = 8f; //X distance between each point on the x axis
        float yMaximum = 60000f;
        float graphHeight = graphContainer.sizeDelta.y;

        GameObject lastCircleGameObject = null;
        for(int i = 0; i < valueList.Count; i++){ //Calculate the positions for each (x,y) in the list
            //X is time

            float xPosition = i > 0 ? i * xSize : 0;

            //Y represents infected
            float yPosition = (valueList[i] / yMaximum) * graphHeight;

            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));

            if(lastCircleGameObject != null){
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            lastCircleGameObject = circleGameObject;

            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -2f);
            labelX.GetComponent<Text>().text = getAxisLabelX(i);

            RectTransform dashX = Instantiate(dashTemplateY);
            dashX.SetParent(graphContainer, false);
            dashX.gameObject.SetActive(true);
            dashX.anchoredPosition = new Vector2(xPosition, -2f);

            }

        int separatorCount = 10;
        for(int i = 0; i <= separatorCount; i++){
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer);
            labelY.gameObject.SetActive(true);
            float normalizedValue = i * 1f / separatorCount;
            labelY.anchoredPosition = new Vector2(-7f, normalizedValue * graphHeight);
            labelY.GetComponent<Text>().text = getAxisLabelY(normalizedValue * yMaximum);

            RectTransform dashY = Instantiate(dashTemplateX);
            dashY.SetParent(graphContainer, false);
            dashY.gameObject.SetActive(true);
            dashY.anchoredPosition = new Vector2(-4f, normalizedValue * graphHeight);
        }
    }
    

    //Create a rectange from dot a to dot b
    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB){
        //Creating new game object, setting the parent, creating the rect transform, giving color
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, .5f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();

        //Creates line vector and makes distance var too
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);


        //Putting anchor on the lower left side(0,0) & (0,0)
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        //Setting it to be a horizontal bar
        rectTransform.sizeDelta = new Vector2(distance, 1.5f);
        //Setting anchor position in middle of A and B 
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;

        //Apply rotation, find the angle from 0 to 360 degrees
        rectTransform.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
        
    }
}
