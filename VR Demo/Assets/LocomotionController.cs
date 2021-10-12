using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    
    //bools to have the teleportation on the empty hand
    public bool EnableLeftTeleport { get; set; } = true;
    public bool EnableRightTeleport { get; set; } = true;
   
    void Update()
    {
        if (leftTeleportRay)
        {
            leftTeleportRay.gameObject.SetActive(EnableLeftTeleport && CheckIfActivated(leftTeleportRay));
        }
        
        if (rightTeleportRay)
        {
            rightTeleportRay.gameObject.SetActive(EnableRightTeleport && CheckIfActivated(rightTeleportRay));
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
