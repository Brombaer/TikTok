using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZombies : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private GameObject[] _zombies;

    private void Start()
    {
        SpawnAI(_zombies, _spawnPoints, false);
    }

    private void SpawnAI(GameObject[] zombies, Transform[] spawnPoints, bool allowOverlap)
    {
        List<GameObject> remainingZombies = new List<GameObject>(zombies);
        List<Transform> freeSpawnPositions = new List<Transform>(spawnPoints);

        if (spawnPoints.Length < zombies.Length)
        {
            Debug.LogWarning(allowOverlap ? "There are more zombies than spawnPoints. Some zombies will overlap." : "There are not enough spawnPoints for all zombies. Some won't spawn.");
        }

        while (remainingZombies.Count > 0)
        {
            if (freeSpawnPositions.Count == 0)
            {
                if (allowOverlap)
                {
                    freeSpawnPositions.AddRange(spawnPoints);
                }
                else
                {
                    break;
                }
            }

            int zombieIndex = Random.Range(0, remainingZombies.Count);
            int spawnPointIndex = Random.Range(0, freeSpawnPositions.Count);

            Instantiate(remainingZombies[zombieIndex], freeSpawnPositions[spawnPointIndex].position, Quaternion.identity, freeSpawnPositions[spawnPointIndex]);
            
            //remainingZombies.RemoveAt(zombieIndex);
            freeSpawnPositions.RemoveAt(spawnPointIndex);
        }
    }
}
