using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
    public bool showController = false;
    [Header("Select the device characteristics")]
    public InputDeviceCharacteristics controllerCharacteristics;
    
    [Header("Controller Prefabs to load when recognised")]
    public List<GameObject> controllerPrefab;
    
    [Header("Hand Prefabs to load")]
    public GameObject handModelPrefab;
    
    
    private InputDevice targetDevice;
    private GameObject spawnedController;
    private GameObject spawnedHandModel;
    private Animator handAnimator;

    [Header("Debug device input")] 
    public bool DebugInputs = false;
    
    void Start()
    {
        //initialise device search
        TryInitialise();
    }

    // Update is called once per frame
    void Update()
    {
        // if in start no device was found (not yet connected on start) try find one.if found stop the search

        if (!targetDevice.isValid)
        {
            TryInitialise();
            Debug.Log("No Device connected yet...");
        }
        else
        {
            // show the hand or controller model accordingly.
            if (showController)
            {
                spawnedHandModel.SetActive(false);
                spawnedController.SetActive(true);
            }
            else
            {
                spawnedHandModel.SetActive(true);
                spawnedController.SetActive(false);
                // update the animations according to the input values of the device
                UpdateHandAnimation();
            }
        }
        
        // see debug input info in console
        if (DebugInputs)
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


    private void TryInitialise()
    {
        // create a list to pass to  InputDevices.GetDevices(L<>);
        List<InputDevice> devices = new List<InputDevice>();
        
        //populate the list with all the devices found with the above characteristics
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics,devices);

        foreach (var item in devices)
        {
            Debug.Log(item.name + item.characteristics);
        }
        
        // check that list is not empty
        if (devices.Count > 0)
        {
            // choose the first device found and apply it 
            targetDevice = devices[0];
            
            // apply the correct model prefab to the device --
            // caution: the prefab name in the project must match the real device name seen in th XR debugger.
            GameObject prefab = controllerPrefab.Find(controller => controller.name == targetDevice.name);
            
            // if there is a prefab, spawn it.
            if (prefab)
            {
                spawnedController = Instantiate(prefab, transform);
            }
            else
            {
                Debug.Log("Did not find the matching model for your controller");
                // apply a default model
                spawnedController = Instantiate(controllerPrefab[0], transform);
            }
            
            // spawn the hand model
            spawnedHandModel = Instantiate(handModelPrefab, transform);
            
            // get the animator of the hands, so to apply the input values for the right animations
            handAnimator = spawnedHandModel.GetComponent<Animator>();
        }
    }
    private void UpdateHandAnimation()
    {
        // get the input values of the device and apply them to the blend tree for the animations
        if(targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handAnimator.SetFloat("Trigger",0);
        }
        
        if(targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip",0);
        }
    }
}
