using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectSO_GameObject
    {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    }
    [SerializeField] private PlateKitchenObject PlateKitchenObject;
    [SerializeField] private List<KitchenObjectSO_GameObject> kitchenObjectSoGameObjectList;

    private void Start()
    {
        PlateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
        foreach (KitchenObjectSO_GameObject kitchenObjectSoGameObject in kitchenObjectSoGameObjectList)
        {
                kitchenObjectSoGameObject.gameObject.SetActive(false);
        }
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        foreach (KitchenObjectSO_GameObject kitchenObjectSoGameObject in kitchenObjectSoGameObjectList)
        {
            if (kitchenObjectSoGameObject.kitchenObjectSO == e.KitchenObjectSo)
            {
                kitchenObjectSoGameObject.gameObject.SetActive(true);
            }
        }
    }
}
