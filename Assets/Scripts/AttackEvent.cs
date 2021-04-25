using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class AttackEvent : MonoBehaviour
{
    private CharacterAiInteractor _characterAiInteractor;

    private void Start()
    {
        _characterAiInteractor = GetComponent<CharacterAiInteractor>();
    }

    public void DamageEvent()
    {
        _characterAiInteractor.Attack();
    }
}
