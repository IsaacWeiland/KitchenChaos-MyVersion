using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenGameManager : MonoBehaviour
{
    public static KitchenGameManager Instance { get; private set; }
    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;
    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }

    private State _state;
    private float countdownToStartTimer = 3f;
    private float gamePlayingTimer;
    private float gamePlayingTimerMax = 20f;
    private bool isGamePaused = false;

    private void Start()
    {
        GameControls.Instance.OnPauseAction += GameControls_OnPauseAction;
        GameControls.Instance.OnInteractAction += GameControls_OnInteractAction;
    }

    private void GameControls_OnInteractAction(object sender, EventArgs e)
    {
        if (_state == State.WaitingToStart)
        {
            _state = State.CountdownToStart;
            OnStateChanged?.Invoke(this,EventArgs.Empty);
        }
    }

    private void Awake()
    {
        Instance = this;
        _state = State.WaitingToStart;
    }

    private void GameControls_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    private void Update()
    {
        switch (_state)
        {
            case State.WaitingToStart:
                
                break;
            case State.CountdownToStart:
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer < 0)
                {
                    _state = State.GamePlaying;
                    gamePlayingTimer = gamePlayingTimerMax;
                    OnStateChanged?.Invoke(this,EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer < 0)
                {
                    _state = State.GameOver;
                    OnStateChanged?.Invoke(this,EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;

        }
    }
    

    public bool IsGamePlaying()
    {
        return _state == State.GamePlaying;
    }

    public bool IsCoundownToStartActive()
    {
        return _state == State.CountdownToStart;
    }

    public float GetCountdownToStartTimer()
    {
        return countdownToStartTimer;
    }
    public bool IsGameOver()
    {
        return _state == State.GameOver;
    }

    public float GetGamePlayingTimerNormalized()
    {
        return 1 - (gamePlayingTimer / gamePlayingTimerMax);
    }

    public void TogglePauseGame()
    {
        isGamePaused = !isGamePaused;
            if(isGamePaused)
            {
                Time.timeScale = 0f;
                OnGamePaused?.Invoke(this,EventArgs.Empty);
            }
            else
            {
                Time.timeScale = 1f;
                OnGameUnpaused?.Invoke(this,EventArgs.Empty);
            }
    }
}
