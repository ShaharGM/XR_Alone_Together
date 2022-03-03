using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class RadioManager : MonoBehaviourPun
{
    public GameObject freqPointer;
    public Light gameObjectLight;
    public GameObject particles;
    public AudioSource audioSource;
    public AudioClip rescueRecording;
    public AudioClip whiteNoise;
    private float[] pointerXPositions = { 0.042f, 0.032f, 0.022f, 0.012f, 0.002f, -0.0075f, -0.0175f, -0.0276f, -0.0375f, -0.0473f, -0.0572f };
    private int currentIndex = 5;
    private int correctFreqIndex = 1;
    private bool stillOnCorrectFreq = false;

    public delegate void EventReaction();
    public static event EventReaction CalledForRescue;
    private void OnEnable()
    {
        // ButtonInteractionManager.ButtonPressed += moveFreqPointer;
        NetworkPlayerSpawner.DeafPlayerSpawned += setDeafPlayer;
    }

    private void OnDisable()
    {
        // ButtonInteractionManager.ButtonPressed -= moveFreqPointer;
        NetworkPlayerSpawner.DeafPlayerSpawned -= setDeafPlayer;
    }

    private void setDeafPlayer()
    {
        if (audioSource != null)
        {
            audioSource.mute = true;
        }
    }

    [PunRPC] public void moveFreqPointer(int moveIndex)
    {
        if ((currentIndex + moveIndex < pointerXPositions.Length) && (currentIndex + moveIndex >= 0))
        {
            // first time turning on the radio
            if (!audioSource.isPlaying)
            {
                StartCoroutine(showingBlindRadio());
            }
            currentIndex += moveIndex;
            // move freq pointer to new current index position
            freqPointer.transform.localPosition = new Vector3(pointerXPositions[currentIndex], freqPointer.transform.localPosition.y, freqPointer.transform.localPosition.z);
            if (currentIndex == correctFreqIndex)
            {
                // play rescue audio
                stillOnCorrectFreq = true;
                audioSource.clip = rescueRecording;
                audioSource.loop = false;
                audioSource.Play();
                StartCoroutine(stayOnCorrectFreq());
            }
            else
            {
                // play white noise
                stillOnCorrectFreq = false;
                audioSource.clip = whiteNoise;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
    }

    private IEnumerator showingBlindRadio()
    {
        while (true)
        {
            GameObject objectCopy = UtilFunctions.CreateCopyWithTransformAndMesh(this.gameObject, this.gameObject.transform.parent);
            GameObject copyObjectLight = Instantiate(gameObjectLight.gameObject, objectCopy.transform);
            GameObject inst_particales = Instantiate(particles, this.transform.position, Quaternion.identity, copyObjectLight.transform);
            UtilFunctions.ChangeLayersRecursively(objectCopy.transform, "Blind Layer");
            yield return new WaitForSeconds(2f);
            Destroy(objectCopy);
            Destroy(copyObjectLight);
        }
    }

    private IEnumerator stayOnCorrectFreq()
    {
        yield return new WaitForSeconds(5f);
        if (stillOnCorrectFreq)
        {
            CalledForRescue?.Invoke();
        }
    }


}
