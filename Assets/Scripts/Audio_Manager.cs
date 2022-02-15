using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Audio_Manager : MonoBehaviour
{
    //IsDeaf parameters
    public AudioMixer Tracks_Mixer;

    public bool Is_Blind;
    public bool Is_Death;

    public string Parameter_To_Change; // should be "Lowpass_Parameter"

    public float Low_Value; // should be 60
    public float High_Value; // should be 22000
    //IsDeaf parameters

    //Snapshot parameters
    public AudioMixerSnapshot Ambience;
    public AudioMixerSnapshot PuzzleMode;

    public bool Transition_To_Ambience = false;
    public bool Transition_To_Puzzle_Mode = false;
    //Snapshot parameters

    
    private void OnEnable()
    {
        AirplaneCrashSiteMonitor.playerArrived += transiteToPuzzleMode;
        NetworkPlayerSpawner.DeafPlayerSpawned += transiteToDeafMode;  
    }

    private void OnDisable()
    {
        AirplaneCrashSiteMonitor.playerArrived -= transiteToPuzzleMode;
        NetworkPlayerSpawner.DeafPlayerSpawned -= transiteToDeafMode;
    }

    private void transiteToPuzzleMode()
    {
        Transition_To_Tape_Working_Function();
    }

    private void transiteToDeafMode()
    {
        Change_LowPass_To_Low_Value();
    }


    //Snapshot Functions
    private void OnValidate()
    {
        if (Transition_To_Ambience == true)
        {
            Transition_To_Ambience = false;
            Transition_To_Ambience_Function();
        }

        if (Transition_To_Puzzle_Mode == true)
        {
            Transition_To_Puzzle_Mode = false;
            Transition_To_Tape_Working_Function();
        }
        ChangeMusicFilterForDeathAndBlind();
    }

    private void Transition_To_Ambience_Function()
    {
        Ambience.TransitionTo(1f);
    }
    private void Transition_To_Tape_Working_Function()
    {
        PuzzleMode.TransitionTo(1f);
    }
    //Snapshot Functions

    //IsDeaf Funcstions
    // Update is called once per frame
    void Update()
    {
        ChangeMusicFilterForDeathAndBlind();
    }

    private void ChangeMusicFilterForDeathAndBlind()
    {
        if (Is_Blind)
        {
            Is_Blind = false;
            Change_LowPass_To_High_Value();
        }
        if (Is_Death)
        {
            Is_Death = false;
            Change_LowPass_To_Low_Value();
        }
    }

    private void Change_LowPass_To_Low_Value()
    {
        Tracks_Mixer.SetFloat(Parameter_To_Change, Low_Value);
    }

    private void Change_LowPass_To_High_Value()
    {
        Tracks_Mixer.SetFloat(Parameter_To_Change, High_Value);
    }
    //IsDeaf
}
