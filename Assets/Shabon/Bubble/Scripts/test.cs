using Shabon.Bubble;
using UnityEngine;
using System;

public class test : MonoBehaviour
{
    public bool isFlag = false;
    IBubbleMono _bubbleMono;

    void Start()
    {
        _bubbleMono = GetComponent<IBubbleMono>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFlag)
        {

            _bubbleMono.InvokeOnDead();

            isFlag = false;
        }
    }
}
