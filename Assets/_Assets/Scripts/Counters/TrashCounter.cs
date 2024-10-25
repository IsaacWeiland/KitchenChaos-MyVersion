using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{

    public static event EventHandler AnyTrash;
    public new static void ResetStaticData()
    {
        AnyTrash = null;
    }
    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            player.GetKitchenObject().DestroySelf();
            AnyTrash?.Invoke(this,EventArgs.Empty);
        }
    }
}
