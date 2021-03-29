using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCamera : MonoBehaviour
{
    [SerializeField] private float _mouseSensitivity = 100;
    [SerializeField] private float _xRotClamp = 90;
    [SerializeField] private Transform _character;

    private CharacterInput _characterInput;

    private void Awake()
    {
        InitializeInput();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void InitializeInput()
    {
        _characterInput = new CharacterInput();
        _characterInput.Player.Look.performed += context => FollowMouse(context.ReadValue<Vector2>());
    }

    private void FollowMouse(Vector2 mouseInput)
    {
        float moveX = mouseInput.x * _mouseSensitivity * Time.deltaTime;
        float moveY = -mouseInput.y * _mouseSensitivity * Time.deltaTime;

        Vector3 rotation = _character.eulerAngles + new Vector3(moveY, moveX, 0);
        rotation.x = ClampAngle(rotation.x, -_xRotClamp, _xRotClamp);
        _character.eulerAngles = rotation;
    }

    private void OnEnable()
    {
        _characterInput.Enable();
    }

    private void OnDisable()
    {
        _characterInput.Disable();
    }

    float ClampAngle(float angle, float start, float end)
    {
        if (angle < 0)
        {
            angle += 360;
        }

        if (angle > 180)
        {
            return Mathf.Max(angle, 360 + start);
        }

        return Mathf.Min(angle, end);
    }
}
