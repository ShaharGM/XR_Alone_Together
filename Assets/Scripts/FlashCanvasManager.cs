using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FlashCanvasManager : MonoBehaviour
{
    public float fadeout_rate = 0.005f;
    public GameObject white_screen;
    private float alphaValue;
    private Image fadeScreen;
    void Start()
    {
        //yield return new WaitForSeconds(fadeout_time);
        fadeScreen = white_screen.GetComponent<Image>();
        alphaValue = fadeScreen.color[3];
    }

    private void FixedUpdate()
    {
        if (alphaValue <= 0)
        {
            Destroy(this.gameObject);
        }

        alphaValue -= fadeout_rate;
        fadeScreen.color = new Color(fadeScreen.color[0], fadeScreen.color[1], fadeScreen.color[2], alphaValue);
    }

}