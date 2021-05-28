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
    [SerializeField] private GameObject _bloodEffect;

    private CharacterMovement _playerMovement;
    private CharacterInteractController _characterInteractController;
    private SphereCollider _sphereCollider;
    private BoxCollider _boxCollider;
    private Vector3 _boxColliderSize;


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
        _boxCollider = GetComponent<BoxCollider>();
        _boxCollider.size = _boxColliderSize;
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
            _boxColliderSize.x = _crouchZombiePerceptionRadius;
            _boxColliderSize.z = _crouchZombiePerceptionRadius;
            _boxCollider.size = _boxColliderSize;
        }
        else if (_playerMovement.GetPlayerStealthProfile() == 1)
        {
            _boxColliderSize.x = _walkZombiePerceptionRadius;
            _boxColliderSize.z = _walkZombiePerceptionRadius;
            _boxCollider.size = _boxColliderSize;
        }
        else if (_playerMovement.GetPlayerStealthProfile() == 2)
        {
            _boxColliderSize.x = _sprintZombiePerceptionRadius;
            _boxColliderSize.z = _sprintZombiePerceptionRadius;
            _boxCollider.size = _boxColliderSize;
        }
        else
        {
            _boxColliderSize.x = 1f;
            _boxColliderSize.z = 1f;
            _boxCollider.size = _boxColliderSize;
        }
    }
    
    private void InitializeInput()
    {
        _characterInput = new CharacterInput();

        _characterInput.Player.Attack.performed += context => _animator.SetTrigger("attack");
    }

    public void Attack()
    {
        if (_animator.GetBool("isHoldingWeapon"))
        {
            //_audioSource.PlayOneShot(AttackSound);
            //_animator.SetTrigger("attack");
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
                Instantiate(_bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
    }
    
    public void DamageEvent()
    {
        Attack();
        FMODUnity.RuntimeManager.PlayOneShot("event:/Character/Attack");
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
