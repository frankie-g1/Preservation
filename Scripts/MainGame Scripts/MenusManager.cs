using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenusManager : MonoBehaviour
{
    public GameObject mainGame;
    public GameObject pauseMenu;
    public GameObject policyMenu;
    public GameObject hospitalTree;
    public GameObject restrictionsTree;
    public GameObject PSATree;
    public GameObject travelTree;
    public GameObject sickTree;

    public void showPauseMenu() {
        pauseMenu.SetActive(true);
    }
    public void showPolicyMenu() {
        policyMenu.SetActive(true);
        mainGame.SetActive(false);
    }
    public void showMainGame() {
        mainGame.SetActive(true);
        policyMenu.SetActive(false);
        pauseMenu.SetActive(false);
    }

    public void toMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    public bool getMainGame()
    {
        return mainGame.activeSelf ? true : false;
    }

    public void toHospitalTree() {
        hospitalTree.SetActive(true);
        restrictionsTree.SetActive(false);
        PSATree.SetActive(false);
        travelTree.SetActive(false);
        sickTree.SetActive(false);
    }

    public void toRestrictionsTree() {
        hospitalTree.SetActive(false);
        restrictionsTree.SetActive(true);
        PSATree.SetActive(false);
        travelTree.SetActive(false);
        sickTree.SetActive(false);
    }

    public void toPSATree() {
        hospitalTree.SetActive(false);
        restrictionsTree.SetActive(false);
        PSATree.SetActive(true);
        travelTree.SetActive(false);
        sickTree.SetActive(false);
    }

    public void toTravelTree()
    {
        hospitalTree.SetActive(false);
        restrictionsTree.SetActive(false);
        PSATree.SetActive(false);
        travelTree.SetActive(true);
        sickTree.SetActive(false);
    }

    public void toSickTree()
    {
        hospitalTree.SetActive(false);
        restrictionsTree.SetActive(false);
        PSATree.SetActive(false);
        travelTree.SetActive(false);
        sickTree.SetActive(true);
    }

}
