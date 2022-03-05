using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatManager : MonoBehaviour
{
    public Animator boatAnimator;
    public AudioSource audioSource;

    private void OnEnable()
    {

        RadioManager.CalledForRescue += comeRescue;
        NetworkPlayerSpawner.DeafPlayerSpawned += muteForDeaf;
    }

    private void OnDisable()
    {
        RadioManager.CalledForRescue -= comeRescue;
    }

    private void muteForDeaf()
    {
        audioSource.mute = true;
    }


    private void comeRescue()
    {
     
        boatAnimator.SetTrigger("CalledForRescue");
        audioSource.Play();
    }
}
