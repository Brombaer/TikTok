using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterAiInteractor : MonoBehaviour
{

    [SerializeField] private float _crouchZombiePerceptionRadius = 0.5f;
    [SerializeField] private float _walkZombiePerceptionRadius = 1;
    [SerializeField] private float _sprintZombiePerceptionRadius = 1.5f;
    [SerializeField] private Transform _spherecastSpawn;
    
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip AttackSound;
    [SerializeField] private float _soundIntensity = 5;
    [SerializeField] private LayerMask _aiLayer;

    private CharacterMovement _playerMovement;
    private CharacterInteractController _characterInteractController;
    private SphereCollider _sphereCollider;

    private Animator _animator;
    private CharacterInput _characterInput;

    private void Awake()
    {
        InitializeInput();
    }

    private void Start()
    {
        //_audioSource = GetComponent<AudioSource>();
        _playerMovement = GetComponent<CharacterMovement>();
        _sphereCollider = GetComponent<SphereCollider>();
        _characterInteractController = GetComponent<CharacterInteractController>();
        
        _animator = GetComponent<Animator>();
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

        _characterInput.Player.Attack.performed += context => Attack();
    }

    private void Attack()
    {
        if (_animator.GetBool("isHoldingWeapon"))
        {
            //_audioSource.PlayOneShot(AttackSound);
            _animator.SetTrigger("attack");
            Collider[] zombies = Physics.OverlapSphere(transform.position, _soundIntensity, _aiLayer);

            for (int i = 0; i < zombies.Length; i++)
            {
                zombies[i].GetComponent<AIBehaviour>().OnAware();
            }

            RaycastHit hit;
            if (Physics.SphereCast(_spherecastSpawn.position, 0.5f, _spherecastSpawn.TransformDirection(Vector3.forward), out hit, 2, _aiLayer))
            {
                int damage = _characterInteractController.Attributes[0].Value.ModifiedValue;

                hit.transform.GetComponent<AIBehaviour>().OnHit(damage);
            }
        }
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
