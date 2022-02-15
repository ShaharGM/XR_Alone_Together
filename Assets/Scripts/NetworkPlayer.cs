using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;

public class NetworkPlayer : MonoBehaviour
{
    public Transform personalLight = null;
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;

    public Animator leftHandAnimator;
    public Animator rightHandAnimator;

    private PhotonView photonView;

    private Transform headRig;
    private XRRig xrRig;
    private Transform leftHandRig;
    private Transform rightHandRig;
    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        xrRig = FindObjectOfType<XRRig>();
        headRig = xrRig.transform.Find("Camera Offset/Main Camera");       
        leftHandRig = xrRig.transform.Find("Camera Offset/LeftHand Controller");
        rightHandRig = xrRig.transform.Find("Camera Offset/RightHand Controller");
        if (headRig == null)
        {
            Debug.Log("Cant find head rig!");
        }
        if (leftHandRig == null)
        {
            Debug.Log("Cant find left hand rig!");
        }
        if (rightHandRig == null)
        {
            Debug.Log("Cant find right hand rig!");
        }

        if (photonView.IsMine)
        {
            foreach (var item in GetComponentsInChildren<Renderer>())
            {
                item.enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            head.position = headRig.position;
            head.rotation = headRig.rotation;

            if (personalLight != null)
            {
                personalLight.position = head.position + new Vector3(0, 0.3f, 0);
                personalLight.rotation = head.rotation;
            }

            leftHand.position = leftHandRig.position;
            leftHand.rotation = leftHandRig.rotation;

            rightHand.position = rightHandRig.position;
            rightHand.rotation = rightHandRig.rotation;

            UpdateHandAnimation(InputDevices.GetDeviceAtXRNode(XRNode.LeftHand), leftHandAnimator);
            UpdateHandAnimation(InputDevices.GetDeviceAtXRNode(XRNode.RightHand), rightHandAnimator);
        }
        

    }

    void UpdateHandAnimation(InputDevice targetDevice, Animator handAnimator)
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }
    }
}
