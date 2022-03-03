using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunBlindingEffect : MonoBehaviour
{
    public float change_rate = 0.001f;
    public float min_exposure = 1.4f;
    public float min_sun_size = 5f;

    private bool reach_min_exposure = false;
    private bool reach_min_size = false;

    private void Start()
    {
        RenderSettings.skybox.SetFloat("_Exposure", 3);
        RenderSettings.skybox.SetFloat("_SunSizeConvergence", 1);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (RenderSettings.skybox == null)
        {
            Destroy(this.gameObject);
        }
        reach_min_exposure = (RenderSettings.skybox.GetFloat("_Exposure") <= min_exposure);
        reach_min_size = (RenderSettings.skybox.GetFloat("_SunSizeConvergence") >= min_sun_size);
        if (reach_min_exposure && reach_min_size)
        {
            Destroy(this.gameObject);
        }

        if (!reach_min_exposure) {
            RenderSettings.skybox.SetFloat("_Exposure", RenderSettings.skybox.GetFloat("_Exposure") - change_rate);
        }
        if (!reach_min_size)
        {
            RenderSettings.skybox.SetFloat("_SunSizeConvergence", RenderSettings.skybox.GetFloat("_SunSizeConvergence") + change_rate);
        }
    }
}
