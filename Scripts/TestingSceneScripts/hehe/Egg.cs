using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    public GameObject egg;
    private int[] eggpass = { 4, 2, 3, 1 };
    private int[] enteredPass = { 0, 0, 0, 0 };
    private int passIndex = 0;

    public void enterPass(int k) {
        enteredPass[passIndex] = k;
        Debug.Log("entered: " + k);
        passIndex++;
        if (passIndex >= 4) {
            checkPass();
        }
    }

    private void clearPass() {
        for (int i = 0; i < 4; i++)
        {
            enteredPass[i] = 0;
        }
        passIndex = 0;
    }

    private void checkPass() {
        Debug.Log("checking pass");
        for (int i = 0; i < 4; i++) {
            if (enteredPass[i] != eggpass[i]) {
                clearPass();
                return;
            }
        }
        passIndex = 0;
        egg.SetActive(true);
    }

    private void Update()
    {
        if (egg.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                egg.SetActive(false);
                clearPass();
            }
        }
    }

}
