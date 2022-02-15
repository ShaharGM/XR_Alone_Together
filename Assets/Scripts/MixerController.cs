using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MixerController : MonoBehaviour
{
    public AudioMixer Tracks_Mixer;

    public bool Is_Change_LowPass_To_High_Value;
    public bool Is_Change_LowPass_To_Low_Value;
    //public bool Is_Fadeout;
    //public float Fadeout_Time = 3f;

    public string Parameter_To_Change;

    public float Low_Value;
    public float High_Value;

    //public AnimationCurve VolumeCurve;

    // Update is called once per frame
    void Update()
    {
        if(Is_Change_LowPass_To_High_Value)
        {
            Is_Change_LowPass_To_High_Value = false;
            Change_LowPass_To_High_Value();
        }
        if (Is_Change_LowPass_To_Low_Value)
        {
            Is_Change_LowPass_To_Low_Value = false;
            Change_LowPass_To_Low_Value();
        }
        /*if(Is_Fadeout)
        {
            Is_Fadeout = false;
            Fadeout();
        }*/
    }

    private void Change_LowPass_To_Low_Value()
    {
        Tracks_Mixer.SetFloat(Parameter_To_Change, Low_Value);
    }

    private void Change_LowPass_To_High_Value()
    {
        Tracks_Mixer.SetFloat(Parameter_To_Change, High_Value);
    }

    /*private void Fadeout()
    {
        StartCoroutine(Fadeout_Coroutine());
    }

    IEnumerator Fadeout_Coroutine()
    {
        //yield return null;
        float t = 0f;

        while (t < Fadeout_Time)
        {
            t = t + Time.deltaTime;
            Tracks_Mixer.SetFloat(Parameter_To_Change, VolumeCurve.Evaluate(t / Fadeout_Time)); // Maximum 1 = t / Fadeout_Time.
            yield return null;
        }
    }*/
}
