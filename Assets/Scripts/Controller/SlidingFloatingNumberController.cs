using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Numerics;

public class SlidingFloatingNumberController : MonoBehaviour
{
    [SerializeField]
    Text numberText;
    [SerializeField]
    float animatedTime = 1.5f;

    string format = "{0.0000}";

    private float desiredNumber;
    private float initialNumber;
    private float currentNumber;

    public void setNumber(float _number)
    {
        initialNumber = currentNumber; 
        desiredNumber = _number;
    }

    public void reset()
    {
        initialNumber = desiredNumber = currentNumber = 0;
    }

    public void setFormat(string _format)
    {
        format = _format;
    }

    private void Update()
    {
        if (currentNumber != desiredNumber)
        {
            float delta = (animatedTime * Time.deltaTime) * (desiredNumber - initialNumber);
            if (initialNumber < desiredNumber)
            {
                currentNumber += delta;
                if (currentNumber >= desiredNumber)
                {
                    currentNumber = desiredNumber;
                }
            }
            else
            {
                currentNumber += delta;
                if (currentNumber <= desiredNumber)
                {
                    currentNumber = desiredNumber;
                }
            }

            numberText.text = string.Format(format, currentNumber);
        }
    }
}