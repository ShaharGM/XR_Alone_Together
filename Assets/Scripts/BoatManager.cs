using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatManager : MonoBehaviour
{
    public Animator boatAnimator;

    private void OnEnable()
    {

        RadioManager.CalledForRescue += comeRescue;
    }

    private void OnDisable()
    {
        RadioManager.CalledForRescue -= comeRescue;
    }


    private void comeRescue()
    {
     
         boatAnimator.SetTrigger("CalledForRescue");

    }
}
