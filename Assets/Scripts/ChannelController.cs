using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ChannelController : MonoBehaviour
{
    public AudioMixer Mixer_Ref;

    public bool IsDeaf = true;

    public string Parameter_To_Change = "Exposed Parameter Name";

    public float Low_Value = 70f;
    public float High_Value = 22000f;

    public void Toggle_Channel()
    {
        if (IsDeaf)
        {
            Mixer_Ref.SetFloat(Parameter_To_Change, Low_Value);
        }
        else
        {
            Mixer_Ref.SetFloat(Parameter_To_Change, High_Value);
        }
        IsDeaf = !IsDeaf;
       // Mixer_Ref.SetFloat(Parameter_To_Change, High_Value);
    }
}
