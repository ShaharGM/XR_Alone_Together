using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ButtonInteractionManager : MonoBehaviourPun
{
    public Animator buttonAnimation;
    public bool isLeft;
    public delegate void EventReaction(int rightOrLeft);
    public static event EventReaction ButtonPressed;

    // Update is called once per frame
    private void OnTriggerEnter(Collider collider)
    {
        if (collider != null && collider.tag == "PlayerHand")
        {
            Debug.Log("Button Pressed!!");
            buttonAnimation.SetTrigger("ButtonPressed");
            // ButtonPressed?.Invoke(isLeft ? -1 : 1);

            photonView.RPC("moveFreqPointer", RpcTarget.All, isLeft ? -1 : 1);
        }
    }
}
