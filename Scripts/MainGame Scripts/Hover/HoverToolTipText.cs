using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverToolTipText : MonoBehaviour
{

    public AudioManager AM;
    public AudioClip enterSound;
    public AudioClip clickSound;

    //Shows tooltip
    public void tooltipEnter(string text)
    {
        ToolTip.ShowToolTip_Static(text);
    }

    //Hides tool tip
    public void toolTipExit()
    {
        ToolTip.HideToolTip_Static();
    }

    //Makes a enter button sound effect
    public void enterButtonSound() {
        AM.playSFX(enterSound);
    }

    //Makes a button click sound effect
    public void clickButtonSound()
    {
        AM.playSFX(clickSound);
    }

}
