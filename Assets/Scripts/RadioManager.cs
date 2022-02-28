using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RadioManager : MonoBehaviour
{
    public GameObject freqPointer;
    public GameObject mainKnob;
    public float rightLimit = -0.062f;
    public float leftLimit = 0.047f;
    private float midPoint;
    private float midPointToLimitDist;
    private Vector3 mainKnobStartPosition;
    private Vector3 tmpRotations;
    private float lastZRotation;

    private bool handInteraction = false;
    private XRBaseInteractor interactor;
    public XRGrabInteractable grabInteractor;

    private void OnEnable()
    {
        grabInteractor.selectEntered.AddListener(GrabbedBy);
        grabInteractor.selectExited.AddListener(GrabEnd);
    }
    private void OnDisable()
    {
        grabInteractor.selectEntered.RemoveListener(GrabbedBy);
        grabInteractor.selectExited.RemoveListener(GrabEnd);
    }

    private void GrabbedBy(SelectEnterEventArgs arg0)
    {
        handInteraction = true;
        interactor = grabInteractor.selectingInteractor;
        interactor.GetComponent<XRDirectInteractor>().hideControllerOnSelect = true;
    }

    private void GrabEnd(SelectExitEventArgs arg0)
    {
        handInteraction = false;
    }
    private void Start()
    {
        midPoint = (rightLimit + leftLimit) / 2f;
        midPointToLimitDist = Mathf.Abs(midPoint - rightLimit);
    }

    private void Update()
    {
        if (handInteraction)
        {
            tmpRotations = mainKnob.transform.localEulerAngles;
            tmpRotations.z = interactor.GetComponent<Transform>().eulerAngles.z;
            mainKnob.transform.localEulerAngles = tmpRotations;

            float newPos = mainKnob.transform.localRotation.z * midPointToLimitDist;
            freqPointer.transform.localPosition = new Vector3(midPoint + newPos, freqPointer.transform.localPosition.y, freqPointer.transform.localPosition.z);
        }
        
    }
}
