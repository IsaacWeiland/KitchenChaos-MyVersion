using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;

    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO KitchenObjectSo;
    }
    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;
    private List<KitchenObjectSO> KitchenObjectSOList;

    private void Awake()
    {
        KitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSo)
    {
        if (KitchenObjectSOList.Contains(kitchenObjectSo) || !validKitchenObjectSOList.Contains(kitchenObjectSo))
        {
            //already has this type or is of incompatible type
            return false;
        }
        else
        {
            KitchenObjectSOList.Add(kitchenObjectSo);
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
            {
                KitchenObjectSo = kitchenObjectSo
            });
            return true;
        }
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList()
    {
        return KitchenObjectSOList;
    }
}
