using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUICraftingHandler : MonoBehaviour
{
    [SerializeField] private CraftingRecipe[] _recipes;
    [SerializeField] private GameObject _reference;
    [SerializeField] private GameObject _craftingRecipeStartSpawnPosition;

    private void Awake()
    {
        for (int i = 0; i < _recipes.Length; i++)
        {
            var tempObject = Instantiate(_reference, _craftingRecipeStartSpawnPosition.transform);
            tempObject.GetComponent<PlayerUICraftingRecipe>().AssignCraftingRecipe(_recipes[i]);
        }
    }
}
