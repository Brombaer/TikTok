using UnityEngine;
using Cinemachine;

public class CinemachinePOVExtension : CinemachineExtension
{
    public float _mouseSensitivity = 10f;
    
    [SerializeField] private float _clampAngle = 90f;
    [SerializeField] private Vector3 _startingRotation;

    private CharacterInput _characterInput;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
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
                if (_characterInput != null)
                {
                    Vector2 deltaInput = _characterInput.Player.Look.ReadValue<Vector2>();
                
                    _startingRotation.x += deltaInput.x * _mouseSensitivity * deltaTime;
                    _startingRotation.y += deltaInput.y * _mouseSensitivity * deltaTime;

                    _startingRotation.y = Mathf.Clamp(_startingRotation.y, -_clampAngle, _clampAngle);
                    state.RawOrientation = Quaternion.Euler(-_startingRotation.y, _startingRotation.x, 0f);
                }
            }
        }
    }

    private void OnEnable()
    {
        _characterInput.Enable();
    }
    
    private void OnDisable()
    {
        _characterInput.Disable();
    }
}

