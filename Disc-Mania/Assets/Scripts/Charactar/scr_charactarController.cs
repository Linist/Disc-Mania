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

    private void Awake()
    {
        defaultInput = new Default_inputs();

        defaultInput.Character.Movement.performed += e => input_Movement = e.ReadValue<Vector2>();
        defaultInput.Character.View.performed += e => input_View = e.ReadValue<Vector2>();

        defaultInput.Enable();

        newCameraRotation = CameraHolder.localRotation.eulerAngles;
        newCharacterRotation = transform.localRotation.eulerAngles;

        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        CalculateView();
        CalculateMovement();
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
    }
}
