using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private float _fieldOfView = 120;
    [SerializeField] private float _viewDistance = 15;
    [SerializeField] private bool _isAware = false;

    [SerializeField] private Transform _head;

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

    public  void OnAware()
    {
        _isAware = true;
    }
}
