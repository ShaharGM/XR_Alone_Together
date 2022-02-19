using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class flashCanvasManager : MonoBehaviour
{
    public float fadeout_time = 0.01f;
    public GameObject white_screen;
    private float alphaValue;
    private Image fadeScreen;
    IEnumerator Start()
    {
        //yield return new WaitForSeconds(fadeout_time);
        fadeScreen = white_screen.GetComponent<Image>();
        alphaValue = fadeScreen.color[3];

        while (alphaValue >= 0)
        {
            alphaValue -= 0.005f;
            yield return new WaitForSeconds(fadeout_time*Time.deltaTime);
            fadeScreen.color = new Vector4(fadeScreen.color[0], fadeScreen.color[1], fadeScreen.color[2], alphaValue);
        }
        
        white_screen.SetActive(false);
    }

}
