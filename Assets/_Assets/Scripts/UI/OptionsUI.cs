using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }
    
    #region ButtonFields
    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicLevelButton;
    [SerializeField] private Button closeOptionsButton;
    [SerializeField] private Button moveUpRebindButton;
    [SerializeField] private Button moveDownRebindButton;
    [SerializeField] private Button moveLeftRebindButton;
    [SerializeField] private Button moveRightRebindButton;
    [SerializeField] private Button interactRebindButton;
    [SerializeField] private Button interactAltRebindButton;
    [SerializeField] private Button pauseRebindButton;
    [SerializeField] private Button gamepadInteractRebindButton;
    [SerializeField] private Button gamepadInteractAltRebindButton;
    [SerializeField] private Button gamepadPauseRebindButton;
    #endregion
    #region TextMeshProUGUIFields
    [SerializeField] private TextMeshProUGUI soundEffectsText;
    [SerializeField] private TextMeshProUGUI musicLevelText;
    [SerializeField] private TextMeshProUGUI moveUpRebindText;
    [SerializeField] private TextMeshProUGUI moveDownRebindText;
    [SerializeField] private TextMeshProUGUI moveLeftRebindText;
    [SerializeField] private TextMeshProUGUI moveRightRebindText;
    [SerializeField] private TextMeshProUGUI interactRebindText;
    [SerializeField] private TextMeshProUGUI interactAltRebindText;
    [SerializeField] private TextMeshProUGUI pauseRebindText;
    [SerializeField] private TextMeshProUGUI gamepadInteractRebindText;
    [SerializeField] private TextMeshProUGUI gamepadInteractAltRebindText;
    [SerializeField] private TextMeshProUGUI gamepadPauseRebindText;
    #endregion

    [SerializeField] private Transform pressToRebindKeyTransform;
    private Action onCloseButtonAction;
    private void Awake()
    {
        Instance = this;
        soundEffectsButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        musicLevelButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        closeOptionsButton.onClick.AddListener(() =>
        {
            Hide();
            onCloseButtonAction();
        });

        moveUpRebindButton.onClick.AddListener(() => { RebindBinding(GameControls.Binding.Move_Up); });
        moveDownRebindButton.onClick.AddListener(() => { RebindBinding(GameControls.Binding.Move_Down); });
        moveLeftRebindButton.onClick.AddListener(() => { RebindBinding(GameControls.Binding.Move_Left); });
        moveRightRebindButton.onClick.AddListener(() => { RebindBinding(GameControls.Binding.Move_Right); });
        interactRebindButton.onClick.AddListener(() => { RebindBinding(GameControls.Binding.Interact); });
        interactAltRebindButton.onClick.AddListener(() => { RebindBinding(GameControls.Binding.InteractAlt); });
        pauseRebindButton.onClick.AddListener(() => { RebindBinding(GameControls.Binding.Pause); });
        gamepadInteractRebindButton.onClick.AddListener(() => { RebindBinding(GameControls.Binding.Gamepad_Interact); });
        gamepadInteractAltRebindButton.onClick.AddListener(() => { RebindBinding(GameControls.Binding.Gamepad_InteractAlt); });
        gamepadPauseRebindButton.onClick.AddListener(() => { RebindBinding(GameControls.Binding.Gamepad_Pause); });
    }

    private void Start()
    {
        KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_OnGameUnpaused;
        UpdateVisual();
        Hide();
        pressToRebindKeyTransform.gameObject.SetActive(false);
    }

    private void KitchenGameManager_OnGameUnpaused(object sender, EventArgs e)
    {
        Hide();
    }

    private void UpdateVisual()
    {
        soundEffectsText.text = "Sound Effects:" + Mathf.Round(SoundManager.Instance.GetVolume() * 10f);
        musicLevelText.text = "Music Level:" + Mathf.Round(MusicManager.Instance.GetVolume() * 10f);

        moveUpRebindText.text = GameControls.Instance.GetBindingText(GameControls.Binding.Move_Up);
        moveDownRebindText.text = GameControls.Instance.GetBindingText(GameControls.Binding.Move_Down);
        moveLeftRebindText.text = GameControls.Instance.GetBindingText(GameControls.Binding.Move_Left);
        moveRightRebindText.text = GameControls.Instance.GetBindingText(GameControls.Binding.Move_Right);
        interactRebindText.text = GameControls.Instance.GetBindingText(GameControls.Binding.Interact);
        interactAltRebindText.text = GameControls.Instance.GetBindingText(GameControls.Binding.InteractAlt);
        pauseRebindText.text = GameControls.Instance.GetBindingText(GameControls.Binding.Pause);
        gamepadInteractRebindText.text = GameControls.Instance.GetBindingText(GameControls.Binding.Gamepad_Interact);
        gamepadInteractAltRebindText.text = GameControls.Instance.GetBindingText(GameControls.Binding.Gamepad_InteractAlt);
        gamepadPauseRebindText.text = GameControls.Instance.GetBindingText(GameControls.Binding.Gamepad_Pause);
    }

    public void Show(Action onCloseButtonAction)
    {
        this.onCloseButtonAction = onCloseButtonAction;
        gameObject.SetActive(true);
        soundEffectsButton.Select();
    }
    public void Hide()
    { 
        gameObject.SetActive(false);
    }

    private void ShowPressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(true);
    }
    private void HidePressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(false);
    }

    private void RebindBinding(GameControls.Binding binding)
    {
        ShowPressToRebindKey();
        GameControls.Instance.RebindBinding(binding, () =>
        {
            HidePressToRebindKey();
            UpdateVisual();
        });
    }
}
