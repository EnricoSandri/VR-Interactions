using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
    public InputDevice targetDevice;
    void Start()
    {
        // create a list to pass to  InputDevices.GetDevices(L<>);
        List<InputDevice> devices = new List<InputDevice>();

        // Create characteristics of the device to search for.
        InputDeviceCharacteristics rightHandCharacteristics =
            InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
       
        //populate the list with all the devices found with the above characteristics
        InputDevices.GetDevicesWithCharacteristics(rightHandCharacteristics,devices);

        foreach (var item in devices)
        {
            Debug.Log(item.name + item.characteristics);
        }
        
        // check that list is not empty
        if (devices.Count > 0)
        {
            // choose the first device found and apply it 
            targetDevice = devices[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        // check for inputs from the device; buttons, triggers and vector2.
        
        // button -- check if it exists - out value so we can utilise it for later use.
        if(targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue) && primaryButtonValue);
        Debug.Log("Pressing primary button");

        // trigger-- check if it exists - out value so we can utilise it for later use.
        if(targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue) && triggerValue > .01f);
        Debug.Log("Trigger pressed " + triggerValue);

        // touchpad-- check if it exists - out value so we can utilise it for later use.
        if(targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisValue)&& primary2DAxisValue != Vector2.zero);
        Debug.Log("Primary touchpad "+ primary2DAxisValue);
    }
}
