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

    //MUSIC
    public AudioSource WaterMusic;
    public AudioSource EndGame;
    //Play the music
    bool m_Play;
    //Detect when you use the toggle, ensures music isn’t played multiple times
    bool m_ToggleChange;
    //

    //Snapshot parameters
    public AudioMixerSnapshot Ambience;
    public AudioMixerSnapshot PuzzleMode;
    public AudioMixerSnapshot WalkInJungle;
    public AudioMixerSnapshot Water;
    public AudioMixerSnapshot End;

    public bool Transition_To_Ambience = false;
    public bool Transition_To_Puzzle_Mode = false;
    public bool Transition_To_JungleWalk = false;
    public bool Transition_To_Water = false;
    public bool Transition_To_End = false;
    //Snapshot parameters

    private void OnEnable()
    {
        AreaMonitorManager.playerArrived += transitionToNewSoundtack;
        NetworkPlayerSpawner.DeafPlayerSpawned += transiteToDeafMode;
    }

    private void OnDisable()
    {
        AreaMonitorManager.playerArrived -= transitionToNewSoundtack;
        NetworkPlayerSpawner.DeafPlayerSpawned -= transiteToDeafMode;
    }

    private void transitionToNewSoundtack(int areaIndex)
    {
        switch ((AreaMonitorManager.Area) areaIndex)
        {
            case AreaMonitorManager.Area.AIRPLANE_CRASH: Transition_To_Tape_Working_Function(); break;
            case AreaMonitorManager.Area.JUNGLE: Transition_To_JungleWalk_Function(); break;
            case AreaMonitorManager.Area.LAKE: Transition_To_Water_Function(); break;
            case AreaMonitorManager.Area.BEACH: Transition_To_End_Function(); break;
            default: break;
        }
    }

    private void transiteToDeafMode()
    {
        Change_LowPass_To_Low_Value();
    }

    private void OnValidate()
    {
        if(Transition_To_Water != true)
        {
            m_ToggleChange = true;
        }

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

        if (Transition_To_JungleWalk == true)
        {
            Transition_To_JungleWalk = false;
            Transition_To_JungleWalk_Function();
        }

        if (Transition_To_Water == true)
        {
            Transition_To_Water = false;
            Transition_To_Water_Function();
        }

        if (Transition_To_End == true)
        {
            Transition_To_End = false;
            Transition_To_End_Function();
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
    private void Transition_To_JungleWalk_Function()
    {
        WalkInJungle.TransitionTo(1f);
    }
    private void Transition_To_Water_Function()
    {
        Water.TransitionTo(1f);

        //Fetch the AudioSource from the GameObject
        //Ensure the toggle is set to true for the music to play at start-up
        //m_Play = true;
        WaterMusic.Play();

    }
    private void Transition_To_End_Function()
    {
        End.TransitionTo(1f);
        EndGame.Play();
    }
    //Snapshot Functions

    //IsDeaf Funcstions
    // Update is called once per frame
    void Update()
    {
        ChangeMusicFilterForDeathAndBlind();

        //Check to see if you just set the toggle to positive
        if (m_Play == true && m_ToggleChange == true)
        {
            //Play the audio you attach to the AudioSource component
            WaterMusic.Play();
            //Ensure audio doesn’t play more than once
            m_ToggleChange = false;
        }
        //Check if you just set the toggle to false
        if (m_Play == false && m_ToggleChange == true)
        {
            //Stop the audio
            WaterMusic.Stop();
            //Ensure audio doesn’t play more than once
            m_ToggleChange = false;
        }
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
