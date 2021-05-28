using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnGroundItems : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private GameObject[] _groundItems;
    [SerializeField] private QuestManager questManager;

    [SerializeField] private bool _allowOverlap = false;
    [SerializeField] private bool _allowSimilarItems = false;

    private float _timer = 3;

    private void Start()
    {
        SpawnItems(_groundItems, _spawnPoints, _allowOverlap, _allowSimilarItems);
    }

    private void SpawnItems(GameObject[] items, Transform[] spawnPoints, bool allowOverlap, bool allowSimilarItems)
    {
        List<GameObject> remainingItems = new List<GameObject>(items);
        List<Transform> freeSpawnPoints = new List<Transform>(spawnPoints);
        
        if (spawnPoints.Length < items.Length)
        {
            Debug.LogWarning(allowOverlap ? "There are more items than spawnpoints. Some items will overlap." : "There are not enough spawnpoints for all items. Some won't spawn.");
        }

        while (remainingItems.Count > 0)
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
        
            int itemIndex = Random.Range(0, remainingItems.Count);
            int spawnPointIndex = Random.Range(0, freeSpawnPoints.Count);
        
            if (allowSimilarItems)
            {
                GameObject gameobject = Instantiate(GetObjectWithMaxProb(), freeSpawnPoints[spawnPointIndex].position, Quaternion.identity, freeSpawnPoints[spawnPointIndex]);
                QuestPickupable pickupable = gameobject.GetComponent<QuestPickupable>();

                if (pickupable != null)
                {
                    pickupable.qManager = questManager;
                }

                Instantiate(GetObjectWithMaxProb(), freeSpawnPoints[spawnPointIndex].position, Quaternion.identity, freeSpawnPoints[spawnPointIndex]);
            }
            else
            {
                GameObject gameobject = Instantiate(remainingItems[itemIndex], freeSpawnPoints[spawnPointIndex].position, Quaternion.identity, freeSpawnPoints[spawnPointIndex]);
                QuestPickupable pickupable = gameobject.GetComponent<QuestPickupable>();

                if (pickupable != null)
                {
                    pickupable.qManager = questManager;
                }

                Instantiate(remainingItems[itemIndex], freeSpawnPoints[spawnPointIndex].position, Quaternion.identity, freeSpawnPoints[spawnPointIndex]);
            }
            
            freeSpawnPoints.RemoveAt(spawnPointIndex);
        }
    }
    
   private GameObject GetObjectWithMaxProb()
   {
       int totalSpawnRate = _groundItems.Sum(t => t.GetComponent<GroundItem>().itemInfo.SpawnRate);
       int randomNumber = Random.Range(0, totalSpawnRate);
       
       GameObject tempObject = null;

       foreach (var item in _groundItems)
       {
           if (randomNumber < item.GetComponent<GroundItem>().itemInfo.SpawnRate)
           {
               tempObject = item;
               break;
           }

           randomNumber -= item.GetComponent<GroundItem>().itemInfo.SpawnRate;
       }

       return tempObject;
   }
}
