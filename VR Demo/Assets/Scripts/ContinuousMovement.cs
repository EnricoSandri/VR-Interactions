using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
public class ContinuousMovement : MonoBehaviour
{
    public XRNode inputSource;
   
    [Header("Character movement")] 
    public float speed = 1;

    public float aditionalHeight = .02f;
    public float gravity = -9.81f;
    public LayerMask groundLayer;
    
    private float fallingSpeed;
    private Vector2 inputAxis;
    private XRRig rig;
    private CharacterController characterController;


    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        rig = GetComponent<XRRig>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get the input value of the controller stick
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
    }

    private void FixedUpdate()
    {
        CapsuleFollowHeadset();
        
        Quaternion headYaw = Quaternion.Euler(0,rig.cameraGameObject.transform.eulerAngles.y,0);
        Vector3 direction = headYaw * new Vector3(inputAxis.x, 0, inputAxis.y);
        
        characterController.Move(direction * Time.fixedDeltaTime * speed);
        
        //gravity
        bool isGrounded = CheckIfGrounded();
        if (isGrounded)
        {
            fallingSpeed = 0;
        }
        else
        {
            fallingSpeed += gravity * Time.fixedDeltaTime;
        }
        
        characterController.Move(Vector3.up * fallingSpeed * Time.fixedDeltaTime);

    }

    bool CheckIfGrounded()
    {
        Vector3 rayStart = transform.TransformPoint(characterController.center);
        float rayLenght = characterController.center.y + 0.01f;
        
        bool hasHit = Physics.SphereCast(rayStart, characterController.radius, Vector3.down, out RaycastHit hitInfo, rayLenght,
            groundLayer);
        return hasHit;
    }

   void CapsuleFollowHeadset()
   {
       characterController.height = rig.cameraInRigSpaceHeight + aditionalHeight;
       Vector3 capsuleCenter = transform.InverseTransformPoint(rig.cameraGameObject.transform.position);
       characterController.center = new Vector3(capsuleCenter.x,
           characterController.height / 2 + characterController.skinWidth, capsuleCenter.z);
   }
}
