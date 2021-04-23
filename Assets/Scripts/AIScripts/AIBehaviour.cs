using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private float _fieldOfView = 120;
    [SerializeField] private float _viewDistance = 10;
    [SerializeField] private bool _isAware = false;

    private NavMeshAgent _agent;

    public void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
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
        }
    }

    private void SearchForPlayer()
    {
        if (Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(_player.transform.position)) < _fieldOfView / 2)
        {
            if (Vector3.Distance(_player.transform.position, transform.position) < _viewDistance)
            {
                OnAware();
            }
        }
    }

    public  void OnAware()
    {
        _isAware = true;
    }
}
