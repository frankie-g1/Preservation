using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberPopup : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    private float disappearTimer;
    private Color textColor;
    private static int sortingOrder;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshProUGUI>();
    }

    public void setUp(string amnt, Color color)
    {
        textMesh.SetText(amnt);
        textMesh.color = color;
        textColor = color;
        disappearTimer = 0.7f;
    }

    public static NumberPopup Create(GameObject pfNumberPopup, Vector3 position, string amnt, Color color) {
        Transform numberPopupTransform = Instantiate(pfNumberPopup, position, Quaternion.identity).transform;
        NumberPopup numberPopup = numberPopupTransform.GetComponent<NumberPopup>();
        GameObject parent = GameObject.Find("MainGameScreen");
        numberPopupTransform.SetParent(parent.transform);
        numberPopup.setUp(amnt, color);
        return numberPopup;
    }

    private void Update()
    {
        float moveYSpeed = 20f;
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;
        disappearTimer -= Time.deltaTime;

        if (disappearTimer < 0) {
            float dissapearSpeed = 10f;
            textColor.a -= dissapearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0) {
                Destroy(gameObject);
            }
        }
    }

}
