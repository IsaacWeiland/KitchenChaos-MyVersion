using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CuttingCounterVisual : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private CuttingCounter cuttingCounter;
    private const string CUT = "Cut";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        cuttingCounter.OnCut += ContainerCounterOnCut;
    }

    private void ContainerCounterOnCut(object sender, EventArgs e)
    {
        _animator.SetTrigger(CUT);
    }
}
