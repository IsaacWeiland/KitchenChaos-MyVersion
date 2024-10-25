using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private GameObject stoveOnGameObject;
    [SerializeField] private GameObject particlesGameObject;
    [SerializeField] private StoveCounter stoveCounter;

    private void Start()
    {
        stoveCounter.OnStateChange += StoveCounterOnOnStateChange;
    }

    private void StoveCounterOnOnStateChange(object sender, StoveCounter.OnStateChangeEventArgs e)
    {
        bool showVisual = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;
        stoveOnGameObject.SetActive(showVisual);
        particlesGameObject.SetActive(showVisual);
    }
}
