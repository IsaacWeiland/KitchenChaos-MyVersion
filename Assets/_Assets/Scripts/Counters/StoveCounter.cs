using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangeEventArgs> OnStateChange;
    public class OnStateChangeEventArgs : EventArgs
    {
        public State state;
    }
    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }
    [SerializeField] private FryingRecipeSO[] fryingRecipeSoArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSoArray;

    private State state;
    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO _fryingRecipeSO;
    private BurningRecipeSO _burningRecipeSo;

    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = fryingTimer / _fryingRecipeSO.fryingTimerMax
                    });
                    if (fryingTimer > _fryingRecipeSO.fryingTimerMax)
                    {
                        //fried
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(_fryingRecipeSO.output, this);
                        state = State.Fried;
                        burningTimer = 0f;
                        _burningRecipeSo = GetBurningingRecipeSO(GetKitchenObject().GetKitchenObjectSo());
                        OnStateChange?.Invoke(this, new OnStateChangeEventArgs
                        {
                            state = state
                        });
                    }
                    break;
                case State.Fried:
                    burningTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = burningTimer / _burningRecipeSo.burningTimerMax
                    });
                    if (burningTimer > _burningRecipeSo.burningTimerMax)
                    {
                        //fried
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(_burningRecipeSo.output, this);
                        state = State.Burned;
                        OnStateChange?.Invoke(this,new OnStateChangeEventArgs
                        {
                            state =  state
                        });
                        
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                    break;
                case State.Burned:
                    break;
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //there is not KitchenObject
            if (player.HasKitchenObject())
            {
                //player is carrying something
                if(HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSo()))
                {
                   player.GetKitchenObject().SetKitchenObjectParent(this);

                   _fryingRecipeSO = GetFryingRecipeSO(GetKitchenObject().GetKitchenObjectSo());
                   state = State.Frying;
                   fryingTimer = 0f;
                   OnStateChange?.Invoke(this, new OnStateChangeEventArgs
                   {
                       state = state
                   });
                   OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                   {
                       progressNormalized = fryingTimer / _fryingRecipeSO.fryingTimerMax
                   });
                }
            }
            else
            {
                //player has nothing
            }
        }
        else
        {
            //there is not KitchenObject
            if (player.HasKitchenObject())
            {
                //player is carrying something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    //player is holding a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSo()))
                    {
                        GetKitchenObject().DestroySelf();
                        
                        state = State.Idle;
                        OnStateChange?.Invoke(this, new OnStateChangeEventArgs
                        {
                            state = state
                        });
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                }
            }
            else
            {
                //player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);

                state = State.Idle;
                OnStateChange?.Invoke(this, new OnStateChangeEventArgs
                {
                    state = state
                });
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
        }
    }
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSO(inputKitchenObjectSO);
        return fryingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSo = GetFryingRecipeSO(inputKitchenObjectSO);
        if (fryingRecipeSo != null)
        {
            return fryingRecipeSo.output;
        }
        else
        {
            return null;
        }
    }

    private FryingRecipeSO GetFryingRecipeSO(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSo in fryingRecipeSoArray)
        {
            if (fryingRecipeSo.input == inputKitchenObjectSO)
            {
                return fryingRecipeSo;
            }
        }

        return null;
    }
    private BurningRecipeSO GetBurningingRecipeSO(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO burningingRecipeSo in burningRecipeSoArray)
        {
            if (burningingRecipeSo.input == inputKitchenObjectSO)
            {
                return burningingRecipeSo;
            }
        }

        return null;
    }

    public bool IsFried()
    {
        return state == State.Fried;
    }
}
