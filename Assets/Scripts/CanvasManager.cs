using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public float seconds_until_ui_disappears = 5f;
    public GameObject get_water_text;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(seconds_until_ui_disappears);
        get_water_text.SetActive(false);
    }
}
