using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Launch : MonoBehaviour
{
    public void toGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
}
