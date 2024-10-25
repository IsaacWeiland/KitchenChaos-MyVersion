using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    public static event EventHandler AnyPlace;

    public static void ResetStaticData()
    {
        AnyPlace = null;
    }
    [SerializeField] private Transform counterTopPoint;
    protected KitchenObject _kitchenObject;
    public virtual void Interact(Player player)
    {
        Debug.LogError("BaseCounter.Interact();");
    } 
    
    public virtual void InteractAlt(Player player)
    {
        //Debug.LogError("BaseCounter.InteractAlt();");
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        _kitchenObject = kitchenObject;
        if (kitchenObject != null)
        {
            AnyPlace?.Invoke(this,EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return _kitchenObject;
    }

    public void ClearKitchenObject()
    {
        _kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return _kitchenObject != null;
    }
}
