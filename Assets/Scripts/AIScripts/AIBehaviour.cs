using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AIBehaviour : MonoBehaviour
{
    private enum MovementType
    {
        Random,
        Waypoint
    };
    
    [SerializeField] private GameObject _playerHead;
    [SerializeField] private float _fieldOfView = 120;
    [SerializeField] private float _viewDistance = 15;
    
    [SerializeField] private float _walkSpeed = 1;
    [SerializeField] private float _chaseSpeed = 2;

    [SerializeField] private float _looseThreshold = 10;
    [SerializeField] private float _moveRadius = 3;
    [SerializeField] private MovementType _movementType = MovementType.Random;

    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private Transform _head;

    private bool _isAware = false;
    private bool _isDetecting = false;
    private float _looseTimer = 0;
    
    private Animator _animator;
    private int _currentWaypointIndex = 0;
    private Vector3 _movePosition;
    private NavMeshAgent _agent;

    public void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _movePosition = RandomMovePosition();
    }

    public void Update()
    {
        SearchForPlayer();
        
        if (_isAware)
        {
            _agent.SetDestination(_playerHead.transform.position);
            _animator.SetBool("isAware", true);
            _agent.speed = _chaseSpeed;
            
            if (_isDetecting == false)
            {
                _looseTimer += Time.deltaTime;

                if (_looseTimer >= _looseThreshold)
                {
                    _isAware = false;
                    _looseTimer = 0;
                }
            }
        }
        else
        {
            Move();
            _animator.SetBool("isAware", false);
            _agent.speed = _walkSpeed;
        }
    }

    private void SearchForPlayer()
    {
        if (Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(_playerHead.transform.position)) < _fieldOfView / 2)
        {
            if (Vector3.Distance(_playerHead.transform.position, transform.position) < _viewDistance)
            {
                RaycastHit hit;
                
                if (Physics.Linecast(_head.position, _playerHead.transform.position, out hit, -1))
                {
                    Debug.DrawLine(_head.position, _playerHead.transform.position, Color.magenta);
                    
                    if (hit.transform.CompareTag("Player"))
                    {
                        OnAware();
                    }
                    else
                    {
                        _isDetecting = false;
                    }
                }
                else
                {
                    _isDetecting = false;
                }
            }
            else
            {
                _isDetecting = false;
            }
        }
        else
        {
            _isDetecting = false;
        }
    }

    private void Move()
    {
        if (_movementType == MovementType.Random)
        {
            if (Vector3.Distance(transform.position, _movePosition) < 2)
            {
                _movePosition = RandomMovePosition();
            }
            else
            {
                _agent.SetDestination(_movePosition);
            }
        }
        else
        {
            if (_waypoints.Length >= 2)
            {
                if (Vector3.Distance(_waypoints[_currentWaypointIndex].position, transform.position) < 2)
                {
                    if (_currentWaypointIndex == _waypoints.Length - 1)
                    {
                        _currentWaypointIndex = 0;
                    }
                    else
                    {
                        _currentWaypointIndex++;
                    }
                }
                else
                {
                    _agent.SetDestination(_waypoints[_currentWaypointIndex].position);
                }
            }
            else
            {
                Debug.LogWarning("Use at least 2 waypoints for the AI's movement" + gameObject.name);
            }
        }
    }

    private Vector3 RandomMovePosition()
    {
        Vector3 randomPosition = (Random.insideUnitSphere * _moveRadius) + transform.position;
        NavMeshHit navMeshHit;
        NavMesh.SamplePosition(randomPosition, out navMeshHit, _moveRadius, -1);
        
        return new Vector3(navMeshHit.position.x, transform.position.y, navMeshHit.position.z);
    }

    public  void OnAware()
    {
        _isAware = true;
        _isDetecting = true;
        _looseTimer = 0;
    }
}
