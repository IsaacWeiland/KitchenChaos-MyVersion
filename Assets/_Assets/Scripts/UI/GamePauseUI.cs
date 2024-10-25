using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    public static GamePauseUI Instance { get; private set; }
    [SerializeField] private Button unpauseButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button optionsButton;

    private void Awake()
    {
        Instance = this;
        unpauseButton.onClick.AddListener(() =>
        {
            KitchenGameManager.Instance.TogglePauseGame();
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        optionsButton.onClick.AddListener(() =>
        {
            OptionsUI.Instance.Show(Show);
            Hide();
        });
    }

    private void Start()
    {
        KitchenGameManager.Instance.OnGamePaused += KitchenGameManager_OnGamePaused;
        KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_OnGameUnpaused;
        
        Hide();
    }

    private void KitchenGameManager_OnGameUnpaused(object sender, EventArgs e)
    {
        Hide();
    }

    private void KitchenGameManager_OnGamePaused(object sender, EventArgs e)
    {
        Show();
    }

    public void Show()
    {
       gameObject.SetActive(true); 
       unpauseButton.Select();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
