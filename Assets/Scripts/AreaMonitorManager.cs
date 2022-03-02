using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaMonitorManager : MonoBehaviour
{
    public delegate void EventReaction(int areaIndex);
    public static event EventReaction playerArrived;

    public enum Area { AIRPLANE_CRASH, JUNGLE, LAKE, BEACH }

    public Area area;

    private void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "Player") && (playerArrived != null))
        {
            playerArrived((int)area);
        }
    }
}
