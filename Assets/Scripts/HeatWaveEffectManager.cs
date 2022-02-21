using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HeatWaveEffectManager : MonoBehaviour
{
    public float fadeout_time = 5f;
    IEnumerator Start()
    {
        yield return new WaitForSeconds(fadeout_time);
        Destroy(this.gameObject);
    }

}