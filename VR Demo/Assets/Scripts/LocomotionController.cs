using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.XR.Interaction.Toolkit;

public class LocomotionController : MonoBehaviour
{
    
    //reference to the rays
    public XRController leftTeleportRay;
    public XRController rightTeleportRay;
    // the button to check
    public InputHelpers.Button teleportActivationButton;
    // the threshold when its activated
    public float activationThreshold = 0.1f;
    
    //bools to check if UI intractor ray, if so disable teleportation
    public XRRayInteractor leftHandInteractorRay;
    public XRRayInteractor rightHandInteractorRay;
    
    //bools to have the teleportation on the empty hand
    public bool EnableLeftTeleport { get; set; } = true;
    public bool EnableRightTeleport { get; set; } = true;
   
    void Update()
    {
        Vector3 pos = new Vector3();
        Vector3 norm = new Vector3();
        int index = 0;
        bool validTarget = false;
        
        if (leftTeleportRay)
        {
            bool isLeftIntractorRayHovering = leftHandInteractorRay.TryGetHitInfo(ref pos,ref norm, ref index, ref validTarget);
            leftTeleportRay.gameObject.SetActive(EnableLeftTeleport && CheckIfActivated(leftTeleportRay) && !isLeftIntractorRayHovering);
        }
        
        if (rightTeleportRay)
        {
            bool isRightIntractorRayHovering = rightHandInteractorRay.TryGetHitInfo(ref pos, ref norm, ref index, ref validTarget);
            rightTeleportRay.gameObject.SetActive(EnableRightTeleport && CheckIfActivated(rightTeleportRay) && !isRightIntractorRayHovering);
        }
    }

    // return a bool when the button is pressed on the controller
    public bool CheckIfActivated(XRController controller)
    {
        // used input helpers here as a shortcut to what we done it the the other class
        InputHelpers.IsPressed(controller.inputDevice, teleportActivationButton, out bool isActivated,
            activationThreshold);
        return isActivated;
    }
}
