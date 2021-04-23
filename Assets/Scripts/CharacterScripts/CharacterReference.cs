using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterReference : MonoBehaviour
{
    private CharacterInput _characterInput;
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private float _crouchZombiePerceptionRadius = 0.5f;
    [SerializeField] private float _walkZombiePerceptionRadius = 1;
    [SerializeField] private float _sprintZombiePerceptionRadius = 1.5f;
    
    // For Weapon sounds 
    //public AudioClip ShootSound;
    //[SerializeField] private float SoundIntensity = 5;
    //[SerializeField] private LayerMask AILayer;

    private CharacterMovement _playerMovement;
    private SphereCollider _sphereCollider;

    private void Awake()
    {
        InitializeInput();
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _playerMovement = GetComponent<CharacterMovement>();
        _sphereCollider = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        CheckPlayerStealthProfile();
    }

    private void CheckPlayerStealthProfile()
    {
        if (_playerMovement.GetPlayerStealthProfile() == 0)
        {
            _sphereCollider.radius = _crouchZombiePerceptionRadius;
        }
        else if (_playerMovement.GetPlayerStealthProfile() == 1)
        {
            _sphereCollider.radius = _walkZombiePerceptionRadius;
        }
        else if (_playerMovement.GetPlayerStealthProfile() == 2)
        {
            _sphereCollider.radius = _sprintZombiePerceptionRadius;
        }
        else
        {
            _sphereCollider.radius = 0.2f;
        }
    }
    
    private void InitializeInput()
    {
        _characterInput = new CharacterInput();

        _characterInput.Player.Attack.performed += context => Fire();
    }

    private void Fire()
    {
        //Debug.Log("Correct method got called");
        //_audioSource.PlayOneShot(ShootSound);
        //Collider[] zombies = Physics.OverlapSphere(transform.position, SoundIntensity, AILayer);
//
        //for (int i = 0; i < zombies.Length; i++)
        //{
        //    zombies[i].GetComponent<AIBehaviour>().OnAware();
        //}
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Zombie"))
        {
            other.GetComponent<AIBehaviour>().OnAware();
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
