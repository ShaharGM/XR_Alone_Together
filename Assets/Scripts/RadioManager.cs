using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RadioManager : MonoBehaviour
{
    public GameObject freqPointer;
    public AudioSource audioSource;
    public AudioClip rescueRecording;
    public AudioClip whiteNoise;
    private float[] pointerXPositions = { 0.042f, 0.032f, 0.022f, 0.012f, 0.002f, -0.0075f, -0.0175f, -0.0276f, -0.0375f, -0.0473f, -0.0572f };
    private int currentIndex = 5;
    private int correctFreqIndex = 1;
    private void OnEnable()
    {
        ButtonInteractionManager.ButtonPressed += moveFreqPointer;
    }

    private void OnDisable()
    {
        ButtonInteractionManager.ButtonPressed -= moveFreqPointer;
    }

    private void moveFreqPointer(int moveIndex)
    {
        if ((currentIndex + moveIndex < pointerXPositions.Length) && (currentIndex + moveIndex >= 0))
        {
            currentIndex += moveIndex;
            // move freq pointer to new current index position
            freqPointer.transform.localPosition = new Vector3(pointerXPositions[currentIndex], freqPointer.transform.localPosition.y, freqPointer.transform.localPosition.z);
            if (currentIndex == correctFreqIndex)
            {
                // play rescue audio
                audioSource.clip = rescueRecording;
                audioSource.loop = false;
                audioSource.Play();
            }
            else
            {
                // play white noise
                audioSource.clip = whiteNoise;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
    }
}
