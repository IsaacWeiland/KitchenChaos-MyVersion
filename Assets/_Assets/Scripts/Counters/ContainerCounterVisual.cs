using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ContainerCounterVisual : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private ContainerCounter containerCounter;
    private const string OPEN_CLOSE = "OpenClose";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        containerCounter.OnPlayerGrabObject += ContainerCounterOnOnPlayerGrabObject;
    }

    private void ContainerCounterOnOnPlayerGrabObject(object sender, EventArgs e)
    {
        _animator.SetTrigger(OPEN_CLOSE);
    }
}
