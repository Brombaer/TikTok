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
    
    [SerializeField] private GameObject _player;
    [SerializeField] private float _fieldOfView = 120;
    [SerializeField] private float _viewDistance = 15;
    [SerializeField] private bool _isAware = false;
    [SerializeField] private float _moveRadius = 3;
    [SerializeField] private MovementType _movementType = MovementType.Random;

    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private Transform _head;

    private int _currentWaypointIndex = 0;
    private Vector3 _movePosition;
    private NavMeshAgent _agent;

    public void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _movePosition = RandomMovePosition();
    }

    public void Update()
    {
        if (_isAware)
        {
            _agent.SetDestination(_player.transform.position);
        }
        else
        {
            SearchForPlayer();
            Move();
        }
    }

    private void SearchForPlayer()
    {
        if (Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(_player.transform.position)) < _fieldOfView / 2)
        {
            if (Vector3.Distance(_player.transform.position, transform.position) < _viewDistance)
            {
                RaycastHit hit;
                
                if (Physics.Linecast(_head.position, _player.transform.position, out hit, -1))
                {
                    Debug.DrawLine(_head.position, _player.transform.position, Color.red);
                    
                    if (hit.transform.CompareTag("Player"))
                    {
                        OnAware();
                    }
                }
            }
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
    }
}
