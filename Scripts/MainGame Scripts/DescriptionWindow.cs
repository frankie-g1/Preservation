using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionWindow : MonoBehaviour
{
    private static DescriptionWindow instance;

    [SerializeField]
    private Camera uiCamera;

    private Text descriptionTitle;
    private Text descriptionText;
    private Text redText;
    private Button buyButton;
    private Button tutorialButton;
    private Button quitGame;
    private RectTransform backgroundRectTransform;
    private bool mustConfirm;
    private bool gameOver;

    private void Awake()
    {
        instance = this;
        backgroundRectTransform = transform.Find("Panel").GetComponent<RectTransform>();
        descriptionTitle = transform.Find("Title Text").GetComponent<Text>();
        descriptionText = transform.Find("Scroll View").transform.Find("Viewport").transform.Find("Content").transform.Find("Description Text").GetComponent<Text>();
        redText = transform.Find("Requirement Error").GetComponent<Text>();
        buyButton = transform.Find("Buy Button").GetComponent<Button>();
        tutorialButton = transform.Find("Tutorial Button").GetComponent<Button>();
        quitGame = transform.Find("Return To Game").GetComponent<Button>();
        gameOver = false;
        instance.hideDesc();
    }   

    public static void showDescription_static(string name, string description)
    {
        instance.showDescription(name, description);
    }

    public void showDescription(string name, string description)
    {
        instance.transform.gameObject.SetActive(true);
        descriptionTitle.text = name;
        descriptionText.text = description;
        redText.gameObject.SetActive(false);
        buyButton.gameObject.SetActive(true);
        buyButton.interactable = true;
        tutorialButton.gameObject.SetActive(false);
        quitGame.gameObject.SetActive(false);


        //float textPaddingSize = 1f;
        //Vector2 backgroundSize = new Vector2(descriptionText.preferredWidth + textPaddingSize * 2f, descriptionText.preferredHeight + textPaddingSize * 2f);
        //backgroundRectTransform.sizeDelta = backgroundSize;
    }

    public static void showDescription_staticError(string name, string description, string error)
    {
        instance.showDescriptionError(name, description, error);
    }

    public void showDescriptionError(string name, string description, string error)
    {
        instance.transform.gameObject.SetActive(true);
        quitGame.gameObject.SetActive(false);
        descriptionTitle.text = name;
        descriptionText.text = description;
        redText.gameObject.SetActive(true);
        redText.text = error;
        buyButton.gameObject.SetActive(true);
        buyButton.interactable = false;
        tutorialButton.gameObject.SetActive(false);
        quitGame.gameObject.SetActive(false);

        //float textPaddingSize = 4f;
        //Vector2 backgroundSize = new Vector2(descriptionText.preferredWidth + textPaddingSize * 2f, descriptionText.preferredHeight + textPaddingSize * 2f);
        //backgroundRectTransform.sizeDelta = backgroundSize;
    }

    public static void showTutorial_static(int tutorialPage)
    {
        instance.showTutorial(tutorialPage);
    }

    public void showTutorial(int tutorialPage)
    {
        instance.transform.gameObject.SetActive(true);
        quitGame.gameObject.SetActive(false);
        buyButton.gameObject.SetActive(false);
        redText.gameObject.SetActive(false);
        tutorialButton.gameObject.SetActive(true);
        tutorialButton.interactable = true;
        quitGame.gameObject.SetActive(false);
        mustConfirm = true;
        switch (tutorialPage)
        {
            case -4:
                
                descriptionTitle.text = "Hover over the values on the left";
                descriptionText.text = ("These factors on the left represent your success in the game\nYour population depends on you, keep the population from becoming fully infected!");
                
                break;

            case -3:

                descriptionTitle.text = "As each day passes...";
                descriptionText.text = ("Passively:\n\tYour budget grows\n\tYour city happiness fluctuates\n\tAnd your Policy Points grow quicker with higher happiness");

                break;

            case -2:
                descriptionTitle.text = "Therefore...";
                descriptionText.text = "And spending your policy points and money help fight against Covid-19!\n\nPress P to open Policy Tree!";
                break;

            case -1:
                descriptionTitle.text = "Tips";
                descriptionText.text = "The virus picks up very slowly, so make sure to take advantage and not hesitate!\n\nPolicies differing in costs in policy points, cash, and their effectiveness\n\nHave fun! Leave feedback";
                mustConfirm = false;
                break;
            
            default:
                Debug.Log("No tutorial found");
                break;
        }

    }

    public static void loseScreen_static()
    {
        instance.loseScreen();
    }

    private void loseScreen()
    {
        mustConfirm = true; // Avoids clicking off the panel
        instance.transform.gameObject.SetActive(true);
        buyButton.gameObject.SetActive(false);
        redText.gameObject.SetActive(true);
        tutorialButton.gameObject.SetActive(false);
        quitGame.gameObject.SetActive(true);
        descriptionText.text = "";
        redText.text = "Your population is sick! Click Return to Main Menu";
        descriptionTitle.text = ":(";
    }

    public void setLose() //button click
    {
        gameOver = true;
    }

    public static bool getLose_static()
    {
        return instance.gameOver;
    }

    public static void hideDesc_Static()
    {
        instance.hideDesc();

    }

    public void hideDesc()
    {
        instance.transform.gameObject.SetActive(false);
    }

    public void Update() {
        if(!mustConfirm)
        {
            hideIfClickedOutside();
        }
    }

    private bool isMouseInside(GameObject obj) {
        Vector2 mousePos = obj.transform.InverseTransformPoint(Input.mousePosition);
        return ((RectTransform)obj.transform).rect.Contains(mousePos);
    }

    private void hideIfClickedOutside()
    {
        if (Input.GetMouseButton(0) && 
            this.gameObject.activeSelf &&
            !isMouseInside(this.gameObject))
        {
            hideDesc();
        }
    }

}
