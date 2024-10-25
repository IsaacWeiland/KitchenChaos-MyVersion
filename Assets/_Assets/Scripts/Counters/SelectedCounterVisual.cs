using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SelectedCounterVisual : MonoBehaviour
{ 
    [SerializeField] private BaseCounter baseCounter; 
    [SerializeField] private GameObject[] visualGameObjectArray;
    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += PlayerOnOnSelectedCounterChanged;
    }

    private void PlayerOnOnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if (e.selectedCounter == baseCounter)
        {
           Show(); 
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        foreach (var gameObject in visualGameObjectArray)
        {
            gameObject.SetActive(true);    
        }
    }

    private void Hide()
    {
        foreach (var gameObject in visualGameObjectArray)
        {
            gameObject.SetActive(false);
        }
    }
}
