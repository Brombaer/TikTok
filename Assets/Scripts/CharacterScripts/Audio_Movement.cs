using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Movement : MonoBehaviour
{
    private enum CURRENT_TERRAIN { CONCRETE, GRASS, DIRT, SAND, WOOD, METAL, PUDDLE };

    [SerializeField]
    private CURRENT_TERRAIN currentTerrain;


    private FMOD.Studio.EventInstance foosteps;
    private FMOD.Studio.EventInstance jumporland;
    private float offset = 0.5f;


    private void Update()
    {
        DetermineTerrain();
    }

    private void DetermineTerrain()
    {


        RaycastHit hit;

        if (Physics.Raycast(transform.position + (Vector3.up * offset), Vector3.down, out hit, 0.6f))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Grass"))
            {
                currentTerrain = CURRENT_TERRAIN.GRASS;
            }
            else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Dirt"))
            {
                currentTerrain = CURRENT_TERRAIN.DIRT;

            }
            else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Sand"))
            {
                currentTerrain = CURRENT_TERRAIN.SAND;

            }
            else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Wood"))
            {
                currentTerrain = CURRENT_TERRAIN.WOOD;

            }
            else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Metal"))
            {
                currentTerrain = CURRENT_TERRAIN.METAL;

            }

            else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Puddle"))
            {
                currentTerrain = CURRENT_TERRAIN.PUDDLE;

            }
            else
            {
                currentTerrain = CURRENT_TERRAIN.CONCRETE;
            }


            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Surface", (int)currentTerrain);
        }




        }

    public void PlayFootsteps()
    {
        foosteps = FMODUnity.RuntimeManager.CreateInstance("event:/Character/Footsteps");
        //foosteps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        foosteps.start();
        foosteps.release();
    }

    public void PlayCrouch()
    {
        foosteps = FMODUnity.RuntimeManager.CreateInstance("event:/Character/Footsteps");
        foosteps.setParameterByName("Crouch", 1f);
        //foosteps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        foosteps.start();
        foosteps.release();
    }

    public void PlayRunning()
    {
        foosteps = FMODUnity.RuntimeManager.CreateInstance("event:/Character/Footsteps");
        foosteps.setParameterByName("Running", 1f);
        //foosteps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        foosteps.start();
        foosteps.release();
    }

    public void PlayJump()
    {
        jumporland = FMODUnity.RuntimeManager.CreateInstance("event:/Character/Jump_Land");
        jumporland.setParameterByName("JumpOrLand", 0);
       // jumporland.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        jumporland.start();
        jumporland.release();
    }


    public void PlayLand()
    {
        jumporland = FMODUnity.RuntimeManager.CreateInstance("event:/Character/Jump_Land");
        jumporland.setParameterByName("JumpOrLand", 1);
       // jumporland.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        jumporland.start();
        jumporland.release();
    }

}
