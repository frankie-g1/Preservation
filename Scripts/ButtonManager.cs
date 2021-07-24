using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{

    public AudioClip HoverSound;
    public AudioClip PressSound;
    public AudioManager AM;


    public void ButtonOnHover()
    {
        AM.playSFX(HoverSound);
    }

    public void ButtonOnExit()
    {
       
    }

    public void ButtonOnClick()
    {
        AM.playSFX(PressSound);
    }

}
