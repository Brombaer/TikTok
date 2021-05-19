using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachinePOVExtension : CinemachineExtension /* MonoBehaviour */
{
    public float _mouseSensitivity = 10f;
    [SerializeField] private float _clampAngle = 90f;
    
    private CharacterInput _characterInput;
    private Vector3 _startingRotation;

    private void Awake()
    {
        //InitialiseInput();
        _characterInput = new CharacterInput();
        base.Awake();
    }

    public void AdjustMouseSensitivity(float newSensitivity)
    {
        _mouseSensitivity = newSensitivity;
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (vcam.Follow)
        {
            if (stage == CinemachineCore.Stage.Aim)
            {
                if (_startingRotation == null)
                {
                    _startingRotation = transform.localRotation.eulerAngles;
                }
                
                Vector2 deltaInput = _characterInput.Player.Look.ReadValue<Vector2>();

                _startingRotation.x += deltaInput.x * _mouseSensitivity * deltaTime;
                _startingRotation.y += deltaInput.y * _mouseSensitivity * deltaTime;

                _startingRotation.y = Mathf.Clamp(_startingRotation.y, -_clampAngle, _clampAngle);
                state.RawOrientation = Quaternion.Euler(-_startingRotation.y, _startingRotation.x, 0f);
            }
        }
    }

    //private void InitialiseInput()
    //{
    //    _characterInput = new CharacterInput();
    //    _characterInput.Player.Look.performed += context => UpdateMouse(context.ReadValue<Vector2>());
    //}

    //private void UpdateMouse(Vector3 mouseInput)
    //{
    //    float moveX = mouseInput.x * _mouseSensitivity * Time.deltaTime;
    //    float moveY = -mouseInput.y * _mouseSensitivity * Time.deltaTime;
//
    //    Vector3 startRotation = new Vector3(moveY, moveX, 0f);
    //    startRotation.y = Mathf.Clamp(moveY, -_clampAngle, _clampAngle);
    //}

    private void OnEnable()
    {
        _characterInput.Enable();
    }
    
    private void OnDisable()
    {
        _characterInput.Disable();
    }
}

