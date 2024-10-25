using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keyMoveUp;
    [SerializeField] private TextMeshProUGUI keyMoveDown;
    [SerializeField] private TextMeshProUGUI keyMoveLeft;
    [SerializeField] private TextMeshProUGUI keyMoveRight;
    [SerializeField] private TextMeshProUGUI keyInteract;
    [SerializeField] private TextMeshProUGUI keyInteractAlt;
    [SerializeField] private TextMeshProUGUI keyPause;
    [SerializeField] private TextMeshProUGUI gamepadInteract;
    [SerializeField] private TextMeshProUGUI gamepadInteractAlt;
    [SerializeField] private TextMeshProUGUI gamepadPause;

    private void Start()
    {
        GameControls.Instance.OnBindingRebind += GameControls_OnBindingRebind;
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;
        UpdateVisual();
        Show();
    }

    private void KitchenGameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (KitchenGameManager.Instance.IsCoundownToStartActive())
        {
            Hide();
        }
    }

    private void GameControls_OnBindingRebind(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        keyMoveUp.text = GameControls.Instance.GetBindingText(GameControls.Binding.Move_Up);
        keyMoveDown.text = GameControls.Instance.GetBindingText(GameControls.Binding.Move_Down);
        keyMoveLeft.text = GameControls.Instance.GetBindingText(GameControls.Binding.Move_Left);
        keyMoveRight.text = GameControls.Instance.GetBindingText(GameControls.Binding.Move_Right);
        keyInteract.text = GameControls.Instance.GetBindingText(GameControls.Binding.Interact);
        keyInteractAlt.text = GameControls.Instance.GetBindingText(GameControls.Binding.InteractAlt);
        keyPause.text = GameControls.Instance.GetBindingText(GameControls.Binding.Pause);
        gamepadInteract.text = GameControls.Instance.GetBindingText(GameControls.Binding.Gamepad_Interact);
        gamepadInteractAlt.text = GameControls.Instance.GetBindingText(GameControls.Binding.Gamepad_InteractAlt);
        gamepadPause.text = GameControls.Instance.GetBindingText(GameControls.Binding.Gamepad_Pause);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
