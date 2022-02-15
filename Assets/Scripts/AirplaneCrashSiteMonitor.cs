using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneCrashSiteMonitor : MonoBehaviour
{
    public delegate void EventReaction();
    public static event EventReaction playerArrived;
 
    private void OnTriggerEnter(Collider other)
    {
        if((other.tag == "Player") && (playerArrived != null))
        {
            playerArrived();
        }
    }
}
