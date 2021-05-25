using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IKillable
{
    //Variable
    [SerializeField]
    private KillZoneEnteredEffect _effect;

    //Function
    public KillZoneEnteredEffect Effect()
    {
        return _effect;
    }

}
