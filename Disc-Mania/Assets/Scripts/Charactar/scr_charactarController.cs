using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static scr_Models;

public class scr_charactarController : MonoBehaviour
{
    private CharacterController characterController;
    private Default_inputs defaultInput;
    public Vector2 input_Movement;
    public Vector2 input_View;

    private Vector3 newCameraRotation;
    private Vector3 newCharacterRotation;

    [Header("Reference")]
    public Transform CameraHolder;    
    
    [Header("Settings")]
    public PlayerSettingModel playerSettings;
    public float ViewClampYMin = -70;
    public float ViewClampYMax = 80;

    [Header("Gravity")]
    public float gravityAmount;
    public float gravityMin;
    private float playerGravity;

    public Vector3 jumpingForce;
    private Vector3 jumpingForceVelocity;

    [Header("Stance")]
    public PlayerStance playerStance;
    public float playerStanceSmoothing;
    public float cameraStandHeight;
    public float cameraCrouchHeight;
    public float cameraProneHeight;

    private float cameraHeight;
    private float cameraHeightVelocity;

    private void Awake()
    {
        defaultInput = new Default_inputs();

        defaultInput.Character.Movement.performed += e => input_Movement = e.ReadValue<Vector2>();
        defaultInput.Character.View.performed += e => input_View = e.ReadValue<Vector2>();
        defaultInput.Character.Jump.performed += e => Jump();

        defaultInput.Enable();

        newCameraRotation = CameraHolder.localRotation.eulerAngles;
        newCharacterRotation = transform.localRotation.eulerAngles;

        characterController = GetComponent<CharacterController>();

        cameraHeight = CameraHolder.localPosition.y;
    }

    private void Update()
    {
        CalculateView();
        CalculateMovement();
        CalculateJump();
        CalculateCameraHeight();
    }

    private void CalculateView()
    {
        newCharacterRotation.y += playerSettings.ViewXSensitivity * (playerSettings.ViewXInveted ? -input_View.x : input_View.x) * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(newCharacterRotation);

        newCameraRotation.x += playerSettings.ViewYSensitivity * (playerSettings.ViewYInveted ? input_View.y : -input_View.y) * Time.deltaTime;
        newCameraRotation.x = Mathf.Clamp(newCameraRotation.x, ViewClampYMin, ViewClampYMax);


        CameraHolder.localRotation = Quaternion.Euler(newCameraRotation);
    }

    private void CalculateMovement()
    {
        var verticalSpeed = playerSettings.WalkinForwardSpeed * input_Movement.y * Time.deltaTime;
        var horisontalSpeed = playerSettings.WalkinStafeSpeed * input_Movement.x * Time.deltaTime;

        var newMovementSpeed = new Vector3 (horisontalSpeed, 0, verticalSpeed);

        newMovementSpeed = transform.TransformDirection(newMovementSpeed);

        characterController.Move(newMovementSpeed);

        if(playerGravity > gravityMin)
        {
            playerGravity -= gravityAmount * Time.deltaTime;
        }

        if(playerGravity < -0.1f && characterController.isGrounded) 
        {
            playerGravity = -0.1f;
        }


        newMovementSpeed.y += playerGravity;
        newMovementSpeed += jumpingForce * Time.deltaTime;

        characterController.Move(newMovementSpeed);
    }

    private void CalculateJump()
    {
        jumpingForce = Vector3.SmoothDamp(jumpingForce, Vector3.zero, ref jumpingForceVelocity, playerSettings.JumpingFalloff);
    }

    private void CalculateCameraHeight()
    {
        var stanceHeight = cameraStandHeight;

        if( playerStance == PlayerStance.Crouch)
        {
            stanceHeight = cameraHeight;
        }

        if( playerStance == PlayerStance.Prone)
        {
            stanceHeight = cameraHeight;
        }

        cameraHeight = Mathf.SmoothDamp(CameraHolder.localPosition.y, cameraHeight, ref cameraHeightVelocity, playerStanceSmoothing);
        CameraHolder.localPosition = new Vector3(CameraHolder.localPosition.x, cameraHeight, CameraHolder.localPosition.z);
    }

    private void Jump()
    {
        if(!characterController.isGrounded)
        {
            return;
        }

        //Jump
        jumpingForce = Vector3.up * playerSettings.JumpingHeigt;
        playerGravity = 0;
    }


}
