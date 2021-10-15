using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TwoHandInteractable : XRGrabInteractable
{
    public List<XRSimpleInteractable> secondHandGrabPoint = new List<XRSimpleInteractable>();

    private XRBaseInteractor secondHandInteractor;
    private Quaternion attachInitialRotation;

    public bool snapToSecondHand = true;
    private Quaternion initialRotationOffset;

    public enum TwoHandRotationType
    {
        None,
        First,
        Second
    };

    public TwoHandRotationType twoHandRotationType;


    // Start is called before the first frame update
    void Start()
    {
        foreach (var item  in secondHandGrabPoint)
        {
            item.onSelectEntered.AddListener(OnSecondHandGrab);
            item.onSelectExited.AddListener(OnSecondHandRelease);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if (secondHandInteractor && selectingInteractor)
        {
            //Compute the rotation of the object via attach points
            if (snapToSecondHand)
            {
                selectingInteractor.attachTransform.rotation = GetTwoHandRotation();
            }
            else
            {
                selectingInteractor.attachTransform.rotation = GetTwoHandRotation() * initialRotationOffset;
            }
            
        }
        base.ProcessInteractable(updatePhase);
    }

    private Quaternion GetTwoHandRotation()
    {
        Quaternion targetRotation;
        if (twoHandRotationType == TwoHandRotationType.None)
        {
            targetRotation = Quaternion.LookRotation(secondHandInteractor.attachTransform.position - selectingInteractor.attachTransform.position);
        }
        else if (twoHandRotationType ==  TwoHandRotationType.First)
        {
            targetRotation = Quaternion.LookRotation(secondHandInteractor.attachTransform.position - selectingInteractor.attachTransform.position, selectingInteractor.transform.up);
        }
        else
        {
            targetRotation = Quaternion.LookRotation(secondHandInteractor.attachTransform.position - selectingInteractor.attachTransform.position, secondHandInteractor.transform.up);
        }

        return targetRotation;
    }
    

    public void OnSecondHandGrab(XRBaseInteractor interactor)
    {
        Debug.Log("SECOND HAND GRAB");
        secondHandInteractor = interactor;
        initialRotationOffset = Quaternion.Inverse(GetTwoHandRotation())* selectingInteractor.attachTransform.rotation;

    }

    public void OnSecondHandRelease(XRBaseInteractor interactor)
    {
        Debug.Log("SECOND HAND RELEASE");
        secondHandInteractor = null;
        selectingInteractor.attachTransform.localRotation = attachInitialRotation;
    }

    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        Debug.Log("FIRST HAND ENRTER");
        base.OnSelectEntered(interactor);
        attachInitialRotation = interactor.attachTransform.localRotation;

    }

    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        Debug.Log("FIRST HAND EXIT");
        base.OnSelectExited(interactor);
        secondHandInteractor = null;
        interactor.attachTransform.localRotation = attachInitialRotation;
    }

    //TO AVOID SNAP POINT ROTATION PROBLEMS - SHORTEN M4 COLLIDER TO JUST AFTER THE SECOND GRAB AND COMMENT THE BELOW FUNCTION WHICH CHECKS IF IS GRABBED
    
    /*public override bool IsSelectableBy(XRBaseInteractor interactor)
    {
        bool isAlreadyGrabbed = selectingInteractor && !interactor.Equals(selectingInteractor);
        return base.IsSelectableBy(interactor) && !isAlreadyGrabbed;
    }*/
}
