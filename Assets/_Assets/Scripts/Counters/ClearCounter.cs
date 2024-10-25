using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //there is not KitchenObject
            if (player.HasKitchenObject())
            {
                //player is carrying something
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                //player has nothing
            }
        }
        else
        {
            //there is a KitchenObject
            if (player.HasKitchenObject())
            {
                //player is carrying something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    //player is holding a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSo()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
                else
                {
                    //player not holding plate
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        //counter has a plate
                        if(plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSo()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
                        
                    }
                }
            }
            else
            {
                //player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}
