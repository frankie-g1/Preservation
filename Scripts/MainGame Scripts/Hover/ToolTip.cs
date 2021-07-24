using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{

    private static ToolTip instance;

    [SerializeField]
    private Camera uiCamera;

    [SerializeField]
    private RectTransform canvasRectTransform;


    private Text tooltipText;
    private RectTransform backgroundRectTransform;

    private void Awake()
    {
        instance = this;
        backgroundRectTransform = transform.Find("Image").GetComponent<RectTransform>();
        tooltipText = transform.Find("Text").GetComponent<Text>();
        HideToolTip();
    }

    private void Update()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, uiCamera, out localPoint);

        //Makes sure tooltip stays on screen at all times
        if (localPoint.x + backgroundRectTransform.rect.width > canvasRectTransform.rect.width/2)
        {
            transform.localPosition = new Vector2(canvasRectTransform.rect.width/2 - backgroundRectTransform.rect.width, localPoint.y);
        }
        else
        {
            transform.localPosition = localPoint;
        }
    }

    private void ShowToolTip(string tooltipString) {
        gameObject.SetActive(true);

        tooltipText.text = tooltipString;
        float textPaddingSize = 4f;
        Vector2 backgroundSize = new Vector2(tooltipText.preferredWidth + textPaddingSize*2f, tooltipText.preferredHeight + textPaddingSize*2f);
        backgroundRectTransform.sizeDelta = backgroundSize;
        Cursor.visible = (false);
    }

    private void HideToolTip() {
        gameObject.SetActive(false);
        Cursor.visible = (true);
    }

    public static void ShowToolTip_Static(string tooltipString)
    {
        instance.ShowToolTip(tooltipString);
    }

    public static void HideToolTip_Static()
    {
        instance.HideToolTip();
    }

}
