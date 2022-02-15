using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Snapshot_Controller : MonoBehaviour
{
    public AudioMixerSnapshot Ambience;    
    public AudioMixerSnapshot PuzzleMode;
    
    public bool Transition_To_Ambience = false;
    public bool Transition_To_Puzzle_Mode = false;

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
    }

    private void Transition_To_Ambience_Function()
    {
        Ambience.TransitionTo(0.5f);
    }
    private void Transition_To_Tape_Working_Function()
    {
        PuzzleMode.TransitionTo(0.5f);
    }
}
