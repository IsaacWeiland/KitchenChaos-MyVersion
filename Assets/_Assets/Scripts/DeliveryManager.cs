using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq.Expressions;
using UnityEngine;
using Unity.Mathematics;
using Random = UnityEngine.Random;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawn;
    public event EventHandler OnRecipeComplete;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFail;
    public static DeliveryManager Instance { get; set; }
    [SerializeField] private RecipeListSO recipeListSo;
    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipesMax = 4;
    private int successfulRecipesAmount;
    private void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;
            if (KitchenGameManager.Instance.IsGamePlaying() && waitingRecipeSOList.Count < waitingRecipesMax)
            {
                RecipeSO waitingRecipeSO = recipeListSo.RecipeSOList[Random.Range(0, recipeListSo.RecipeSOList.Count)];
                waitingRecipeSOList.Add(waitingRecipeSO);
                
                OnRecipeSpawn?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];
            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
             //has the same number of ingredients 
             bool plateMatchesRecipe = true;
             foreach (KitchenObjectSO rKOSO in waitingRecipeSO.kitchenObjectSOList)
             {
              //cycling through all ingredients in recipe
              bool ingredientFound = false; 
              foreach (KitchenObjectSO pKOSO in plateKitchenObject.GetKitchenObjectSOList())
              {
                  //cycling through all ingredients on plate
                  if (pKOSO == rKOSO)
                  {
                      //ingredients match
                      ingredientFound = true;
                      break;
                  }
              }

              if (!ingredientFound)
              {
               //This Recipe ingredient was not found on the Plate   
               plateMatchesRecipe = false;
              }
             }

             if (plateMatchesRecipe)
             {
                 //player delivered correct recipe
                 successfulRecipesAmount++;
                 waitingRecipeSOList.RemoveAt(i);
                 OnRecipeComplete?.Invoke(this, EventArgs.Empty);
                 OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                 return;
             }
            }
        }
        //No matches found
        //player did not deliever correct recipe
        OnRecipeFail?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }

    public int GetSuccessfulRecipesAmount()
    {
        return successfulRecipesAmount;
    }
}
