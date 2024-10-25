using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour , IKitchenObjectParent
{
    public static Player Instance { get; private set; }

    public event EventHandler OnPickup;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    private KitchenObject _kitchenObject;
    [SerializeField] private Transform kitchenObjectHoldPoint;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }
        
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameControls gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    private bool isWalking;
    private BaseCounter _selectedCounter;

    private Vector3 lastInteractDir;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Player instance.");
        }
        Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInputOnOnInteractAction;
        gameInput.OnInterAltAct += GameInputOnOnInterAltAct;
    }

    private void GameInputOnOnInterAltAct(object sender, EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying())return;
        
        if (_selectedCounter != null)
        {
            _selectedCounter.InteractAlt(this);
        }
        
    }

    private void GameInputOnOnInteractAction(object sender, EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying())return;
        
        if (_selectedCounter != null)
        {
            _selectedCounter.Interact(this);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }
        float interactDist = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDist, countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                //has clearCounter
                if (baseCounter != _selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        _selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs { selectedCounter = selectedCounter});
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f; 
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);
        if (!canMove)
        {
            //cannot move this direction
            //attempt only x movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = (moveDir.x < -.5f || moveDir.x > +.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                //can only move on the X axis
                moveDir = moveDirX;
            }
            else
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = (moveDir.z < -.5f || moveDir.z > +.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canMove)
                {
                    //can only move on the Z axis
                    moveDir = moveDirZ;
                }
                else
                {
                    //cannot move in any direction.
                }
            }
        }
        if (canMove)
        {
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }
        isWalking = moveDir != Vector3.zero;
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    #region IKitchenObjectInherits

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        _kitchenObject = kitchenObject;

        if (kitchenObject != null)
        {
         OnPickup?.Invoke(this,EventArgs.Empty);   
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

    #endregion
}
