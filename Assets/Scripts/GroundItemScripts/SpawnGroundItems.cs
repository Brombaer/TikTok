using System.Collections.Generic;
using UnityEngine;

public class SpawnGroundItems : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private GameObject[] _groundItems;

    private void Start()
    {
        SpawnItems(_groundItems, _spawnPoints, false);
    }

    private void SpawnItems(GameObject[] items, Transform[] spawnPoints, bool allowOverlap)
    {
        List<GameObject> remaningItems = new List<GameObject>(items);
        List<Transform> freeSpawnPoints = new List<Transform>(spawnPoints);

        if (spawnPoints.Length < items.Length)
        {
            Debug.LogWarning(allowOverlap ? "There are more items than spawnpoints. Some items will overlap." : "There are not enough spawnpoints for all items. Some won't spawn.");
        }

        while (remaningItems.Count > 0)
        {
            if (freeSpawnPoints.Count == 0)
            {
                if (allowOverlap)
                {
                    freeSpawnPoints.AddRange(spawnPoints);
                }
                else
                {
                    break;
                }
            }
        
            int itemIndex = Random.Range(0, remaningItems.Count);
            int spawnPointIndex = Random.Range(0, freeSpawnPoints.Count);
        
            Instantiate(remaningItems[itemIndex], freeSpawnPoints[spawnPointIndex].position, Quaternion.identity, freeSpawnPoints[spawnPointIndex]);
        
            remaningItems.RemoveAt(itemIndex);
            freeSpawnPoints.RemoveAt(spawnPointIndex);
        }
    }
}
