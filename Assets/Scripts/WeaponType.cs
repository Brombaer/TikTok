using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponType : MonoBehaviour
{
    private enum CURRENT_WEAPON { BOTTLE, CROWBAR, HAMMER, KATANA, METALBAT, PAN, PIPE, WOODBAT, WOODBATNAILS };

    [SerializeField]
    private CURRENT_WEAPON currentWeapon;


    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "WeaponHandSlot")
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("WeaponType", (int)currentWeapon);
    }

    private void onTriggerExit(Collider other)
    {
        if (other.name == "WeaponHandSlot")
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("WeaponType", 0f);
    }

}
