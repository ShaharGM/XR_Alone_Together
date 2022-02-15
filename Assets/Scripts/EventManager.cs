using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void EventReaction();
    public static event EventReaction AirplaneExplosion;
    public float seconds_until_explosion = 10f;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(seconds_until_explosion);
        if(AirplaneExplosion != null)
        {
            AirplaneExplosion();
        }
    }
}
